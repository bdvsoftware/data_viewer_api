using DataViewerApi.Persistance.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Repository;

public interface IVideoRepository
{
    Task<Video> GetVideo(int videoId);
    Task<Video?> GetVideoByName(string videoName);
    Task<Video> AddVideo(Video video);
}

public class VideoRepository : IVideoRepository
{
    
    public readonly ApplicationDbContext _db;

    public VideoRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Video> GetVideo(int videoId)
    {
        var video = await _db.Videos.FindAsync(videoId);
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
        var video = await _db.Videos.SingleOrDefaultAsync(v => v.Name == videoName);
        return video;
    }

    public async Task<Video> AddVideo(Video video)
    {
        await _db.Videos.AddAsync(video);
        await _db.SaveChangesAsync();
        return video;
    }
}