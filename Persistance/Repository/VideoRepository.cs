using DataViewerApi.Dto;
using DataViewerApi.Persistance.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Repository;

public interface IVideoRepository
{
    Task<Video> GetVideo(int videoId);
    Task UpdateVideo(Video video);
    Task<Video?> GetVideoByName(string videoName);
    Task<Video> AddVideo(Video video);
    Task<IEnumerable<ResponseVideoDto>> GetAllVideos();
    Task<string> FindVideoPathById(int videoId);
    Task<int> GetVideoFrameCount(int videoId);
    Task UpdateVideoStatus(int videoId, string status);
    Task<string> GetVideoPathById(int videoId);
    Task<int> GetVideoFrameRateById(int videoId);
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
            v.Session.Gp.Name,
            v.Status
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

    public async Task UpdateVideo(Video video)
    {
        _db.Videos.Update(video);
        await _db.SaveChangesAsync();
    }

    public async Task<int> GetVideoFrameCount(int videoId)
    {
        return await _db.Videos
            .Where(v => v.VideoId == videoId)
            .Select(v => v.TotalFrames)
            .FirstAsync();
    }

    public async Task UpdateVideoStatus(int videoId, string status)
    {
        var video = await GetVideo(videoId);
        video.Status = status;
        await UpdateVideo(video);
    }

    public async Task<string> GetVideoPathById(int videoId)
    {
        return await _db.Videos
            .Where(v => v.VideoId == videoId)
            .Select(v => v.OriginalPath)
            .FirstAsync();
    }

    public async Task<int> GetVideoFrameRateById(int videoId)
    {
        return await _db.Videos
            .Where(v => v.VideoId == videoId)
            .Select(v => v.FrameRate)
            .FirstAsync();
    }
}