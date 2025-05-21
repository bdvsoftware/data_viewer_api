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
    Task ProduceFrames(int videoId, string videoUrl, int threshold);
    Task ReceiveProcessedFrame(ProcessedFrameDto processedFrame);
}

public class FrameService : IFrameService
{
    private readonly FrameKafkaProducer _frameKafkaProducer;

    private readonly IFrameRepository _frameRepository;
    
    private readonly IVideoRepository _videoRepository;

    private readonly IOnboardHelmetFrameService _onboardHelmetFrameService;

    private readonly IBatteryFrameService _batteryFrameService;
    
    private readonly ILogger<FrameService> _logger;

    public FrameService(FrameKafkaProducer frameKafkaProducer, IFrameRepository frameRepository, IVideoRepository videoRepository, IOnboardHelmetFrameService onboardHelmetFrameService, IBatteryFrameService batteryFrameService, ILogger<FrameService> logger)
    {
        _frameKafkaProducer = frameKafkaProducer;
        _frameRepository = frameRepository;
        _videoRepository = videoRepository;
        _onboardHelmetFrameService = onboardHelmetFrameService;
        _batteryFrameService = batteryFrameService;
        _logger = logger;
    }

    public async Task ProduceFrames(int videoId, string videoUrl, int threshold)
    {

        var capture = new VideoCapture(videoUrl);

        if (!capture.IsOpened())
            throw new System.Exception("Error accessing video");

        double fps = capture.Fps;
        double totalFrames = capture.FrameCount;
        double durationSeconds = totalFrames / fps;

        int frameCount = 0;

        for (int second = 0; second < (int)durationSeconds; second+=threshold)
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
                            videoId,
                            frameCount,
                            second, 
                            null
                        ));

                    var meesage = new FrameToProcessDto(
                        videoId,
                        savedFrame.FrameId,
                        savedFrame.Seq,
                        savedFrame.Timestamp,
                        base64Image
                    );

                    await _frameKafkaProducer.SendMessageAsync(meesage);

                    _logger.LogInformation($"Seq: {savedFrame.Seq}, Frame: {savedFrame.FrameId}");
                    frameCount++;
                }
            }
        }

        capture.Release();
        await UpdateVideoData(videoId, frameCount, durationSeconds, threshold);
    }

    private async Task UpdateVideoData(int videoId, double totalFrames, double durationSeconds, int frameRate)
    {
        var video = await _videoRepository.GetVideo(videoId);
        video.TotalFrames = IntegerType.FromObject(totalFrames);
        video.Duration = IntegerType.FromObject(durationSeconds);
        video.FrameRate = frameRate;
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
            _logger.LogInformation("Processed frame: "+processedFrame.FrameSeq);
        }
        catch (System.Exception ex)
        {
            Console.Write(ex.Message);
        }
        
        try
        {
            var totalVideoFrames = await _videoRepository.GetVideoFrameCount(processedFrame.VideoId);
            if (processedFrame.FrameSeq == totalVideoFrames - 1)
            {
                await _videoRepository.UpdateVideoStatus(processedFrame.VideoId, Constants.VideoStatus.Processed);
                _logger.LogInformation($"Video PROCESSED: {processedFrame.VideoId}");
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, $"Failed to update video status for video {processedFrame.VideoId}");
        }
    }
}