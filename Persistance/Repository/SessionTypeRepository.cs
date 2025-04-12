using DataViewerApi.Persistance.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Repository;

public interface ISessionTypeRepository
{
    Task<IEnumerable<SessionType>> GetAll();
    Task<List<string>> GetAllNames();
}

public class SessionTypeRepository : ISessionTypeRepository
{
    private readonly ApplicationDbContext _db;

    public SessionTypeRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<SessionType>> GetAll()
    {
        return await _db.SessionTypes.ToListAsync();
    }

    public Task<List<string>> GetAllNames()
    {
        return _db.SessionTypes
            .Select(t => t.Name)
            .ToListAsync();
    }
}