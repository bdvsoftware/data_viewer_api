using DataViewerApi.Dto;
using DataViewerApi.Kafka.Consumer;
using DataViewerApi.Kafka.Producer;
using DataViewerApi.Persistance.Repository;
using DataViewerApi.Persistance.Entity;
using DataViewerApi.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DataViewerApi.Service;

public interface IVideoService
{
    Task<Boolean> ExistsVideoByName(string videoName);
    Task<UploadedVideoDto> UploadVideo(RequestUploadVideoDto request);
    Task<IEnumerable<ResponseVideoDto>> GetVideos();
    Task StartVideoProcessing(int videoId);
    Task<Dictionary<string, DriverVideoDto>> GetVideoData(int videoId);
    Task<(Stream Stream, string FileName)> GetVideoFile(int videoId);
    Task<string> GetVideoPath(int videoId);
}

public class VideoService : IVideoService
{
    private readonly IVideoRepository _videoRepository;

    private readonly ISessionRepository _sessionRepository;

    private readonly ISessionTypeRepository _sessionTypeRepository;

    private readonly IGrandPrixRepository _grandPrixRepository;
    
    private readonly IDriverRepository _driverRepository;
    
    private readonly IFrameService _frameService;
    
    private readonly IBatteryFrameDriverRepository _batteryFrameDriverRepository;
    
    private readonly IOnboardFrameRepository _onboardFrameRepository;

    private readonly VideoToProcessKafkaProducer _videoToProcessKafkaProducer;

    public VideoService(IVideoRepository videoRepository, ISessionRepository sessionRepository, ISessionTypeRepository sessionTypeRepository, IGrandPrixRepository grandPrixRepository, IDriverRepository driverRepository, IFrameService frameService, IBatteryFrameDriverRepository batteryFrameDriverRepository, IOnboardFrameRepository onboardFrameRepository, VideoToProcessKafkaProducer videoToProcessKafkaProducer)
    {
        _videoRepository = videoRepository;
        _sessionRepository = sessionRepository;
        _sessionTypeRepository = sessionTypeRepository;
        _grandPrixRepository = grandPrixRepository;
        _driverRepository = driverRepository;
        _frameService = frameService;
        _batteryFrameDriverRepository = batteryFrameDriverRepository;
        _onboardFrameRepository = onboardFrameRepository;
        _videoToProcessKafkaProducer = videoToProcessKafkaProducer;
    }

    public async Task<Boolean> ExistsVideoByName(string videoName)
    {
        var video = await _videoRepository.GetVideoByName(videoName);
        return video is not null;
    }

    public async Task<UploadedVideoDto> UploadVideo(RequestUploadVideoDto request)
    {
        var filePath = Path.Combine(Constants.VideoDirectory, request.File.FileName);

        var gp = await _grandPrixRepository.GetByName(request.GrandPrixName);

        var sessionType = await _sessionTypeRepository.GetByName(request.SessionName);

        var session = await _sessionRepository.AddSession(
            new Session(gp.GpId, sessionType.SessionTypeId, gp.Date.ToDateTime(TimeOnly.MinValue))
        );

        var video = await _videoRepository.AddVideo(
            new Video(session.SessionId, request.File.FileName, filePath, 0, 0, "UPLOADED")
        );
        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream);
            Console.Out.WriteLine("Uploaded video: " + filePath);
        }

        return new UploadedVideoDto(video.VideoId, video.Name, video.Url);
    }

    public async Task<IEnumerable<ResponseVideoDto>> GetVideos()
    {
        return await _videoRepository.GetAllVideos();
    }

    public async Task StartVideoProcessing(int videoId)
    {
        var video = await _videoRepository.GetVideo(videoId);

        if (Constants.VideoStatus.Processed.Equals(video.Status) || Constants.VideoStatus.Processing.Equals(video.Status))
        {
            await DeleteExistingProcessedFrames(video.VideoId);
        }

        var videoToProcess = new VideoToProcessDto(video.VideoId, video.Url);

        await _videoToProcessKafkaProducer.SendMessageAsync(videoToProcess);
        
        await _videoRepository.UpdateVideoStatus(videoId, Constants.VideoStatus.Processing);
    }
    
    public async Task<Dictionary<string, DriverVideoDto>> GetVideoData(int videoId)
    {
        var driverIds = await _driverRepository.GetDriversIds();
        var dict = new Dictionary<string, DriverVideoDto>();
        foreach (var driverId in driverIds)
        {
            var driverNameData = await _driverRepository.GetDriverNameAndAbrreviation(driverId);
            var key = "(" + driverNameData.DriverAbbreviation + ") " + driverNameData.DriverName;
            var onboardData = await _driverRepository.GetDriverOnboardData(driverId, videoId);
            var batteryData = await _driverRepository.GetDriverBatteryData(driverId, videoId);
            var rangeTimeBattery = GroupBatteryDataByTimestampRange(batteryData);
            var rangeTimeOnboard = GroupOnboardDataByTimestampRange(onboardData);
            var driverData = new DriverVideoDto(rangeTimeOnboard, rangeTimeBattery);
            
            dict.TryAdd(key, driverData);
        }
        return dict;
    }

    public async Task<(Stream Stream, string FileName)> GetVideoFile(int videoId)
    {
        var video = await _videoRepository.GetVideo(videoId);

        if (!File.Exists(video.Url))
        {
            throw new FileNotFoundException("Video not found");
        }
        
        var stream = new FileStream(video.Url, FileMode.Open, FileAccess.Read);
        var fileName = video.Name;
        
        return (stream, fileName);
    }

    private async Task DeleteExistingProcessedFrames(int videoId)
    {
        await _batteryFrameDriverRepository.DeleteAllBatteryFrameDriversByVideoId(videoId);
        await _onboardFrameRepository.DeleteAllOnboardFramesByVideoId(videoId);
    }
    
    private IEnumerable<DriverBatteryRangeDto> GroupBatteryDataByTimestampRange(IEnumerable<DriverBatteryDto> batteryData)
    {
        var sortedData = batteryData.OrderBy(b => b.Timestamp).ToList();
        var result = new List<DriverBatteryRangeDto>();

        if (!sortedData.Any()) return result;

        int start = sortedData[0].Timestamp;
        int end = start;

        var first = sortedData[0];

        for (int i = 1; i < sortedData.Count; i++)
        {
            var current = sortedData[i];

            if (current.Timestamp == end + 1)
            {
                end = current.Timestamp;
            }
            else
            {
                result.Add(CreateDriverBatteryRangeDto(first, start, end));
                first = current;
                start = end = current.Timestamp;
            }
        }

        result.Add(CreateDriverBatteryRangeDto(first, start, end));

        return result;
    }
    
    private DriverBatteryRangeDto CreateDriverBatteryRangeDto(DriverBatteryDto dto, int start, int end)
    {
        return new DriverBatteryRangeDto
        (
            dto.DriverName,
            dto.DriverAbbreviation,
            $"{FormatSecondsToTime(start)} - {FormatSecondsToTime(end)}",
            dto.FrameId,
            dto.BatteryFrameId,
            dto.Lap,
            dto.Status);
    }
    
    public List<DriverOnboardRangeDto> GroupOnboardDataByTimestampRange(IEnumerable<DriverOnboardDto> onboardData)
    {
        var sortedData = onboardData.OrderBy(d => d.Timestamp).ToList();
        var result = new List<DriverOnboardRangeDto>();

        if (!sortedData.Any()) return result;

        int start = sortedData[0].Timestamp;
        int end = start;

        var first = sortedData[0];

        for (int i = 1; i < sortedData.Count; i++)
        {
            var current = sortedData[i];

            if (current.Timestamp == end + 1)
            {
                end = current.Timestamp;
            }
            else
            {
                result.Add(CreateDriverOnboardRangeDto(first, start, end));
                first = current;
                start = end = current.Timestamp;
            }
        }

        result.Add(CreateDriverOnboardRangeDto(first, start, end));

        return result;
    }
    
    private DriverOnboardRangeDto CreateDriverOnboardRangeDto(DriverOnboardDto dto, int start, int end)
    {
        return new DriverOnboardRangeDto
        (
            dto.DriverName,
            dto.DriverAbbreviation,
            dto.TeamName,
            dto.OnboardFrameId,
            $"{FormatSecondsToTime(start)} - {FormatSecondsToTime(end)}");
    }
    
    private string FormatSecondsToTime(int totalSeconds)
    {
        var time = TimeSpan.FromSeconds(totalSeconds);
        return time.ToString(@"hh\:mm\:ss");
    }

    public async Task<string> GetVideoPath(int videoId)
    {
        return await _videoRepository.GetVideoPathById(videoId);
    }
}