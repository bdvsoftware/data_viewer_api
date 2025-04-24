using DataViewerApi.Persistance.Entity;

namespace DataViewerApi.Persistance.Repository;

public interface IBatteryFrameDriverRepository
{
    Task<BatteryFrameDriver> AddBatteryFrameDriver(BatteryFrameDriver batteryFrameDriver);
}

public class BatteryFrameDriverRepository : IBatteryFrameDriverRepository
{
    private readonly ApplicationDbContext _db;

    public BatteryFrameDriverRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<BatteryFrameDriver> AddBatteryFrameDriver(BatteryFrameDriver batteryFrameDriver)
    {
        await _db.BatteryFrameDrivers.AddAsync(batteryFrameDriver);
        await _db.SaveChangesAsync();
        return batteryFrameDriver;
    }
}