using DataViewerApi.Persistance.Repository;

namespace DataViewerApi.Service;

public interface IVideoService
{
    Task<Boolean> ExistsVideoByName(string videoName);
}

public class VideoService : IVideoService
{
    private readonly IVideoRepository _videoRepository;

    public VideoService(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    public async Task<Boolean> ExistsVideoByName(string videoName)
    {
        var video = await _videoRepository.GetVideoByName(videoName);
        return video is not null;
    }
}