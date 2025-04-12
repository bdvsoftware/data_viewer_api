using DataViewerApi.Persistance.Repository;
using DataViewerApi.Persistance.Entity;

namespace DataViewerApi.Service;

public interface IGrandPrixService
{
    Task<IEnumerable<GrandPrix>> GetAll();
}

public class GrandPrixService
{
    
    private readonly GrandPrixRepository _grandPrixRepository;

    public GrandPrixService(GrandPrixRepository grandPrixRepository)
    {
        _grandPrixRepository = grandPrixRepository;
    }

    public async Task<IEnumerable<GrandPrix>> GetAll()
    {
        return await _grandPrixRepository.GetAll();
    }
}