using DataViewerApi.Persistance.Entity;

namespace DataViewerApi.Persistance.Repository;

public interface IDrivereyeFrameRepository
{
    Task<DrivereyeFrame> AddDrivereyeFrame(DrivereyeFrame drivereyeFrame);
}

public class DrivereyeFrameRepository : IDrivereyeFrameRepository
{
    private readonly ApplicationDbContext _db;

    public DrivereyeFrameRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<DrivereyeFrame> AddDrivereyeFrame(DrivereyeFrame drivereyeFrame)
    {
        await _db.DrivereyeFrames.AddAsync(drivereyeFrame);
        await _db.SaveChangesAsync();
        return drivereyeFrame;
    }
}