using DataViewerApi.Persistance.Entity;

namespace DataViewerApi.Persistance.Repository;

public interface IBatteryFrameRepository
{
    public Task<BatteryFrame> AddBatteryFrame(BatteryFrame batteryFrame);
}

public class BatteryFrameRepository : IBatteryFrameRepository
{
    private readonly ApplicationDbContext _db;

    public BatteryFrameRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<BatteryFrame> AddBatteryFrame(BatteryFrame batteryFrame)
    {
        await _db.BatteryFrames.AddAsync(batteryFrame);
        await _db.SaveChangesAsync();
        return batteryFrame;
    }
}