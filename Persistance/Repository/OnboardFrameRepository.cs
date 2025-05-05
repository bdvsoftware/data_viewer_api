using DataViewerApi.Persistance.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Repository;

public interface IOnboardFrameRepository
{
    public Task<OnboardFrame> AddOnboardFrame(OnboardFrame onboardFrame);
    public Task DeleteAllOnboardFramesByVideoId(int videoId);
}

public class OnboardFrameRepository : IOnboardFrameRepository
{
    private readonly ApplicationDbContext _db;

    public OnboardFrameRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<OnboardFrame> AddOnboardFrame(OnboardFrame onboardFrame)
    {
        await _db.OnboardFrames.AddAsync(onboardFrame);
        await _db.SaveChangesAsync();
        return onboardFrame;
    }

    public async Task DeleteAllOnboardFramesByVideoId(int videoId)
    {
        var idsToDelete = await (
            from of in _db.OnboardFrames
            join f in _db.Frames on of.FrameId equals f.FrameId
            join v in _db.Videos on f.VideoId equals v.VideoId
            where v.VideoId == videoId
            select of.OnboardFrameId
            ).ToListAsync();
        if (idsToDelete.Any())
        {
            foreach (var id in idsToDelete)
            {
                await _db.OnboardFrames
                    .Where(bfd => bfd.OnboardFrameId.Equals(id))
                    .ExecuteDeleteAsync();
            }
        }
    }
}