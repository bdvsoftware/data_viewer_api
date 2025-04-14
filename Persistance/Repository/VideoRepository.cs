using DataViewerApi.Dto;
using DataViewerApi.Persistance.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Repository;

public interface IVideoRepository
{
    Task<Video> GetVideo(int videoId);
    Task<Video?> GetVideoByName(string videoName);
    Task<Video> AddVideo(Video video);
    Task<IEnumerable<ResponseVideoDto>> GetAllVideos();
    Task<string> FindVideoPathById(int videoId);
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

    public async Task<IEnumerable<ResponseVideoDto>> GetAllVideos()
    {
        var videos = await _db.Videos
            .Include(v => v.Session)
            .ThenInclude(s => s.SessionType)
            .Include(v => v.Session)
            .ThenInclude(s => s.Gp)
            .ToListAsync();

        return videos.Select(v => new ResponseVideoDto(
            v.VideoId,
            v.Name,
            v.Session.SessionId,
            v.Session.SessionType.Name,
            v.Session.Gp.Date,
            v.Session.Gp.Name
        ));
    }

    public async Task<string> FindVideoPathById(int videoId)
    {
        var path = await _db.Videos
            .Select(v => v.Url)
            .FirstOrDefaultAsync();

        if (path is not null)
        {
            return path;
        }

        return "";
    }
}