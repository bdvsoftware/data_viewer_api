using DataViewerApi.Persistance.Entity;

namespace DataViewerApi.Persistance.Repository;

public interface IFrameRepository
{
    Task<Frame> AddFrame(Frame frame);
}

public class FrameRepository : IFrameRepository
{
    
    private readonly ApplicationDbContext _db;

    public FrameRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Frame> AddFrame(Frame frame)
    {
        await _db.Frames.AddAsync(frame);
        await _db.SaveChangesAsync();
        return frame;
    }
}