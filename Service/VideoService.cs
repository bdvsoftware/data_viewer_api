using DataViewerApi.Dto;
using DataViewerApi.Persistance.Repository;
using DataViewerApi.Persistance.Entity;
using DataViewerApi.Utils;

namespace DataViewerApi.Service;

public interface IVideoService
{
    Task<Boolean> ExistsVideoByName(string videoName);
    Task<UploadedVideoDto> UploadVideo(RequestUploadVideoDto request);
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

    public async Task<UploadedVideoDto> UploadVideo(RequestUploadVideoDto request)
    {
        var filePath = Path.Combine(Constants.VideoDirectory, request.File.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream);
            Console.Out.WriteLine("Uploaded video: " + filePath);
        }

        return new UploadedVideoDto(1, "prueba", "/videos");
    }
}