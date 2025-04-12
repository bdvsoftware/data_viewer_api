using DataViewerApi.Persistance.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Repository;

public interface IGrandPrixRepository
{
    Task<IEnumerable<GrandPrix>> GetAll();
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
}