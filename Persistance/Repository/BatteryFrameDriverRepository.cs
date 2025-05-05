using DataViewerApi.Dto;
using DataViewerApi.Persistance.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Repository;

public interface IBatteryFrameDriverRepository
{
    Task<BatteryFrameDriver> AddBatteryFrameDriver(BatteryFrameDriver batteryFrameDriver);
    Task DeleteAllBatteryFrameDriversByVideoId(int videoId);
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

    public async Task DeleteAllBatteryFrameDriversByVideoId(int videoId)
    {
        var idsToDelete = await (
            from bfd in _db.BatteryFrameDrivers
            join bf in _db.BatteryFrames on bfd.BatteryFrameId equals bf.BatteryFrameId
            join f in _db.Frames on bf.FrameId equals f.FrameId
            join v in _db.Videos on f.VideoId equals v.VideoId
            where v.VideoId == videoId
            select new BatteryFrameDriverIdDto(
                bfd.DriverId,
                bfd.BatteryFrameId)
        ).ToListAsync();
        if (idsToDelete.Any())
        {
            foreach (var id in idsToDelete)
            {
                await _db.BatteryFrameDrivers
                    .Where(bfd => bfd.DriverId.Equals(id.DriverId) && bfd.BatteryFrameId.Equals(id.BatteryFrameId))
                    .ExecuteDeleteAsync();
            }
        }
        
    }
}