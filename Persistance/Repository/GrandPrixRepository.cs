using DataViewerApi.Persistance.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Repository;

public interface IGrandPrixRepository
{
    Task<IEnumerable<GrandPrix>> GetAll();
    Task<List<string>> GetAllNames();
    Task<GrandPrix> GetByName(string gpName);
}

public class GrandPrixRepository : IGrandPrixRepository
{
    private readonly ApplicationDbContext _db;

    public GrandPrixRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<GrandPrix>> GetAll()
    {
        return await _db.GrandPrixes.ToListAsync();
    }

    public async Task<List<string>> GetAllNames()
    {
        return await _db.GrandPrixes
            .Select(gp => gp.Name)
            .ToListAsync();
    }

    public async Task<GrandPrix> GetByName(string name)
    {
        return await _db.GrandPrixes
            .Where(gp => gp.Name == name)
            .SingleAsync();
    }
}