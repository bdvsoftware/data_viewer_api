using DataViewerApi.Dto;
using DataViewerApi.Persistance.Repository;
using DataViewerApi.Persistance.Entity;
using DataViewerApi.Utils;

namespace DataViewerApi.Service;

public interface IVideoService
{
    Task<Boolean> ExistsVideoByName(string videoName);
    Task<UploadedVideoDto> UploadVideo(RequestUploadVideoDto request);
    Task<IEnumerable<ResponseVideoDto>> GetVideos();
    Task<List<FrameToProcessDto>> ProcessVideo(int videoId);
}

public class VideoService : IVideoService
{
    private readonly IVideoRepository _videoRepository;

    private readonly ISessionRepository _sessionRepository;

    private readonly ISessionTypeRepository _sessionTypeRepository;

    private readonly IGrandPrixRepository _grandPrixRepository;
    
    private readonly IFrameService _frameService;

    public VideoService(
        IVideoRepository videoRepository,
        ISessionRepository sessionRepository,
        ISessionTypeRepository sessionTypeRepository,
        IGrandPrixRepository grandPrixRepository,
        IFrameService frameService)
    {
        _videoRepository = videoRepository;
        _sessionRepository = sessionRepository;
        _sessionTypeRepository = sessionTypeRepository;
        _grandPrixRepository = grandPrixRepository;
        _frameService = frameService;
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

    public async Task<List<FrameToProcessDto>> ProcessVideo(int videoId)
    {
        var video = await _videoRepository.GetVideo(videoId);
        
        return await _frameService.ProduceFrames(video);
    }
}