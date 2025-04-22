using DataViewerApi.Persistance.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Repository;

public interface IDriverRepository
{
    
    public Task<IEnumerable<string>> GetDriversNames();
    public Task<Driver?> GetDriverByAbbreviation(string abbreviation);
    public Task<Driver?> GetDriverByName(string name);
    public Task<Driver?> GetDriverByLowerCaseName(string name);
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
}