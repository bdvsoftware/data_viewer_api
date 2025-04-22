using DataViewerApi.Persistance.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Repository;

public interface IFrameRepository
{
    Task<Frame> AddFrame(Frame frame);
    Task<Frame> GetFrameById(int id);
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

    public async Task<Frame> GetFrameById(int id)
    {
        return await _db.Frames.FirstAsync(f => f.FrameId == id);
    }
}