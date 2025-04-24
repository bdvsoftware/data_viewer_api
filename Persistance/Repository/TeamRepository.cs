using DataViewerApi.Persistance.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Repository;

public interface ITeamRepository
{
    
}

public class TeamRepository : ITeamRepository
{

    private readonly ApplicationDbContext _db;

    public TeamRepository(ApplicationDbContext db)
    {
        _db = db;
    }
}