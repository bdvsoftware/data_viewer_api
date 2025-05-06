using System.Collections;
using DataViewerApi.Dto;
using DataViewerApi.Persistance.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Repository;

public interface IDriverRepository
{
    public Task<IEnumerable<string>> GetDriversNames();
    public Task<Driver?> GetDriverByAbbreviation(string abbreviation);
    public Task<Driver?> GetDriverByName(string name);
    public Task<Driver?> GetDriverByLowerCaseName(string name);
    public Task<string?> GetDriverTeamNameByAbbreviation(string abbreviation);
    public Task<IEnumerable<int>> GetDriversIds();
    public Task<string?> GetDriverAbbreviationByDriverId(int driverId);
    public Task<IEnumerable<DriverOnboardDto>> GetDriverOnboardData(int driverId, int videoId);
    public Task<IEnumerable<DriverBatteryDto>> GetDriverBatteryData(int driverId, int videoId);
    public Task<DriverNameDto> GetDriverNameAndAbrreviation(int driverId);
}

public class DriverRepository : IDriverRepository
{
    private readonly ApplicationDbContext _db;

    public DriverRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<string>> GetDriversNames()
    {
        return await _db.Drivers
            .Select(driver => driver.Name)
            .ToListAsync();
    }

    public async Task<Driver?> GetDriverByAbbreviation(string abbreviation)
    {
        return await _db.Drivers.FirstOrDefaultAsync(d => d.Abbreviation == abbreviation);
    }

    public Task<Driver?> GetDriverByName(string name)
    {
        return _db.Drivers.FirstOrDefaultAsync(d => d.Name == name);
    }

    public Task<Driver?> GetDriverByLowerCaseName(string lowerCaseName)
    {
        return _db.Drivers.FirstOrDefaultAsync(d => d.Name.ToLower() == lowerCaseName);
    }

    public async Task<string?> GetDriverTeamNameByAbbreviation(string abbreviation)
    {
        return await _db.Drivers
            .Where(d => d.Abbreviation == abbreviation)
            .Select(d => d.Team.Name)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<int>> GetDriversIds()
    {
        return await _db.Drivers
            .Select(d => d.DriverId)
            .ToListAsync();
    }

    public async Task<IEnumerable<DriverOnboardDto>> GetDriverOnboardData(int driverId, int videoId)
    {
        var result = await (
            from driver in _db.Drivers
            join team in _db.Teams on driver.TeamId equals team.TeamId
            join onboard_frame in _db.OnboardFrames on driver.DriverId equals onboard_frame.DriverId
            join frame in _db.Frames on onboard_frame.FrameId equals frame.FrameId
            join video in _db.Videos on frame.VideoId equals video.VideoId
            where driver.DriverId == driverId && video.VideoId == videoId
            select new DriverOnboardDto(
                driver.Name,
                driver.Abbreviation,
                team.Name,
                onboard_frame.OnboardFrameId,
                frame.Timestamp
            )
        ).ToListAsync();

        return result;
    }

    public async Task<IEnumerable<DriverBatteryDto>> GetDriverBatteryData(int driverId, int videoId)
    {
        var result = await (
            from driver in _db.Drivers
            join battery_frame_driver in _db.BatteryFrameDrivers on driver.DriverId equals battery_frame_driver.DriverId
            join battery_frame in _db.BatteryFrames on battery_frame_driver.BatteryFrameId equals battery_frame
                .BatteryFrameId
            join frame in _db.Frames on battery_frame.FrameId equals frame.FrameId
            join video in _db.Videos on frame.VideoId equals video.VideoId
            join team in _db.Teams on driver.TeamId equals team.TeamId
            where driver.DriverId == driverId && video.VideoId == videoId && battery_frame_driver.Status != -1
            select new DriverBatteryDto(
                driver.Name,
                driver.Abbreviation,
                frame.Timestamp,
                frame.FrameId,
                battery_frame.BatteryFrameId,
                frame.Lap,
                battery_frame_driver.Status
            )).ToListAsync();
        return result;
    }

    public async Task<string?> GetDriverAbbreviationByDriverId(int driverId)
    {
        return await _db.Drivers
            .Where(d => d.DriverId == driverId)
            .Select(d => d.Abbreviation)
            .FirstOrDefaultAsync();
    }

    public async Task<DriverNameDto> GetDriverNameAndAbrreviation(int driverId)
    {
        var result = await (
            from driver in _db.Drivers
            where driver.DriverId == driverId
            select new DriverNameDto(
                driver.Name,
                driver.Abbreviation
            )).FirstAsync();
        return result;
    }
}