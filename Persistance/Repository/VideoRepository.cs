using DataViewerApi.Prueba;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Repository;

public interface IVideoRepository
{
    Task<Video> GetVideo(int videoId);
    Task<Video?> GetVideoByName(string videoName);
    Task AddVideo(Video video);
}

public class VideoRepository : IVideoRepository
{
    
    private readonly ApplicationDbContext _context;

    public VideoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Video> GetVideo(int videoId)
    {
        var video = await _context.Videos.FindAsync(videoId);
        if (video is not null)
        {
            return video;
        }
        else
        {
            throw new KeyNotFoundException("Video not found");
        }
    }

    public async Task<Video?> GetVideoByName(string videoName)
    {
        var video = await _context.Videos.SingleOrDefaultAsync(v => v.Name == videoName);
        return video;
    }

    public async Task AddVideo(Video video)
    {
        await _context.Videos.AddAsync(video);
    }
}