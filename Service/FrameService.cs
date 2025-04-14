using System.Drawing;
using System.Drawing.Imaging;
using DataViewerApi.Dto;
using DataViewerApi.Persistance.Entity;
using DataViewerApi.Persistance.Repository;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace DataViewerApi.Service;

public interface IFrameService
{
    Task<List<FrameToProcessDto>> ProduceFrames(Video video);
}

public class FrameService : IFrameService
{
    private readonly FrameKafkaProducer _frameKafkaProducer;

    private readonly IFrameRepository _frameRepository;

    public FrameService(FrameKafkaProducer frameKafkaProducer, IFrameRepository frameRepository)
    {
        _frameKafkaProducer = frameKafkaProducer;
        _frameRepository = frameRepository;
    }

    public async Task<List<FrameToProcessDto>> ProduceFrames(Video video)
    {
        var frames = new List<FrameToProcessDto>();
        
        var capture = new VideoCapture(video.Url);

        if (!capture.IsOpened())
            throw new Exception("No se pudo abrir el video.");

        double fps = capture.Fps;
        double totalFrames = capture.FrameCount;
        double durationSeconds = totalFrames / fps;

        for (int second = 0; second < (int)durationSeconds; second++)
        {
            int targetFrame = (int)(second * fps);
            capture.Set(VideoCaptureProperties.PosFrames, targetFrame);

            using (var frame = new Mat())
            {
                if (!capture.Read(frame) || frame.Empty())
                    continue;

                using (Bitmap bmp = BitmapConverter.ToBitmap(frame))
                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Png);
                    string base64Image = Convert.ToBase64String(ms.ToArray());

                    var savedFrame = await _frameRepository.AddFrame(
                        new Frame(
                            video.VideoId,
                            second,
                            second
                        ));

                    var meesage = new FrameToProcessDto(
                        video.VideoId,
                        savedFrame.FrameId,
                        savedFrame.Seq,
                        savedFrame.Timestamp,
                        base64Image
                    );

                    await _frameKafkaProducer.SendMessageAsync(meesage);
                    
                    frames.Add(meesage);
                }
            }
        }

        capture.Release();
        return frames;
    }
}