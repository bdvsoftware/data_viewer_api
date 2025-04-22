using DataViewerApi.Persistance.Entity;

namespace DataViewerApi.Persistance.Repository;

public interface IOnboardFrameRepository
{
    public Task<OnboardFrame> AddOnboardFrame(OnboardFrame onboardFrame);
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
    
}