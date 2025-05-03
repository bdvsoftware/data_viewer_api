using System.Drawing;
using System.Drawing.Imaging;
using DataViewerApi.Dto;
using System;
using DataViewerApi.Persistance.Entity;
using DataViewerApi.Persistance.Repository;
using DataViewerApi.Utils;
using Microsoft.VisualBasic.CompilerServices;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace DataViewerApi.Service;

public interface IFrameService
{
    Task<List<FrameToProcessDto>> ProduceFrames(Video video);
    Task ReceiveProcessedFrame(ProcessedFrameDto processedFrame);
}

public class FrameService : IFrameService
{
    private readonly FrameKafkaProducer _frameKafkaProducer;

    private readonly IFrameRepository _frameRepository;
    
    private readonly IVideoRepository _videoRepository;

    private readonly IOnboardHelmetFrameService _onboardHelmetFrameService;

    private readonly IBatteryFrameService _batteryFrameService;

    public FrameService(FrameKafkaProducer frameKafkaProducer, IFrameRepository frameRepository, IVideoRepository videoRepository, IOnboardHelmetFrameService onboardHelmetFrameService, IBatteryFrameService batteryFrameService)
    {
        _frameKafkaProducer = frameKafkaProducer;
        _frameRepository = frameRepository;
        _videoRepository = videoRepository;
        _onboardHelmetFrameService = onboardHelmetFrameService;
        _batteryFrameService = batteryFrameService;
    }

    public async Task<List<FrameToProcessDto>> ProduceFrames(Video video)
    {
        var frames = new List<FrameToProcessDto>();

        var capture = new VideoCapture(video.Url);

        if (!capture.IsOpened())
            throw new System.Exception("Error accessing video");

        double fps = capture.Fps;
        double totalFrames = capture.FrameCount;
        double durationSeconds = totalFrames / fps;

        int frameCount = 1;

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
                    bmp.Save(ms, ImageFormat.Jpeg);
                    string base64Image = Convert.ToBase64String(ms.ToArray());

                    var savedFrame = await _frameRepository.AddFrame(
                        new Frame(
                            video.VideoId,
                            second,
                            second, 
                            null
                        ));

                    var meesage = new FrameToProcessDto(
                        video.VideoId,
                        savedFrame.FrameId,
                        savedFrame.Seq,
                        savedFrame.Timestamp,
                        base64Image
                    );

                    await UpdateVideoFramesAndDuration(video.VideoId, frameCount, durationSeconds);

                    await _frameKafkaProducer.SendMessageAsync(meesage);

                    frames.Add(meesage);
                    frameCount++;
                }
            }
        }

        capture.Release();
        await _videoRepository.UpdateVideoStatus(video.VideoId, Constants.VideoStatus.Processing);
        return frames;
    }

    private async Task UpdateVideoFramesAndDuration(int videoId, double totalFrames, double durationSeconds)
    {
        var video = await _videoRepository.GetVideo(videoId);
        video.TotalFrames = IntegerType.FromObject(totalFrames);
        video.Duration = IntegerType.FromObject(durationSeconds);
        await _videoRepository.UpdateVideo(video);
    }

    public async Task ReceiveProcessedFrame(ProcessedFrameDto processedFrame)
    {
        try
        {
            if (processedFrame.OnboardHelmetDto != null)
            {
                await _onboardHelmetFrameService.ProcessOnboardHelmetFrame(
                    processedFrame.FrameId,
                    processedFrame.OnboardHelmetDto
                );
            }

            if (processedFrame.BatteryDriverDataDto != null)
            {
                await _batteryFrameService.ProcessBatteryFrame(
                    processedFrame.FrameId,
                    processedFrame.BatteryDriverDataDto
                );
            }
            
            var totalVideoFrames = await _videoRepository.GetVideoFrameCount(processedFrame.VideoId);
            if (processedFrame.FrameSeq.Equals(totalVideoFrames-1))
            {
                await _videoRepository.UpdateVideoStatus(processedFrame.VideoId, Constants.VideoStatus.Processed);
            }
        }
        catch (System.Exception ex)
        {
            Console.Write(ex.Message);
        }
    }
}