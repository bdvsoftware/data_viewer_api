using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Repository;

public interface ITokenConsumptionRepository
{
    Task DeleteTokenConsumptionByFrames(IEnumerable<int> frameIds);
}

public class TokenConsumptionRepository : ITokenConsumptionRepository
{
    
    private readonly ApplicationDbContext _db;

    public TokenConsumptionRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task DeleteTokenConsumptionByFrames(IEnumerable<int> frameIds)
    {
        var frameIdList = frameIds.ToArray();

        var tokenConsumptionsToDelete = await _db.TokenConsumptions
            .Where(tc => frameIdList.Contains(tc.FrameId))
            .ToListAsync();

        _db.TokenConsumptions.RemoveRange(tokenConsumptionsToDelete);
        await _db.SaveChangesAsync();
    }
}