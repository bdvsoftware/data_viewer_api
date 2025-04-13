using DataViewerApi.Persistance.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Repository;

public interface ISessionRepository
{
    Task<IEnumerable<Session>> GetAll();
    Task<Session> AddSession(Session session);
}

public class SessionRepository : ISessionRepository
{
    
    private readonly ApplicationDbContext _db;

    public SessionRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Session>> GetAll()
    {
        return await _db.Sessions.ToListAsync();
    }

    public async Task<Session> AddSession(Session session)
    {
        await _db.Sessions.AddAsync(session);
        await _db.SaveChangesAsync();
        return session;
    }

}