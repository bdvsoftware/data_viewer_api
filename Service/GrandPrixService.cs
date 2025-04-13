using DataViewerApi.Persistance.Repository;
using DataViewerApi.Persistance.Entity;

namespace DataViewerApi.Service;

public interface IGrandPrixService
{
    Task<IEnumerable<GrandPrix>> GetAll();
    Task<List<string>> GetAllNames();
}

public class GrandPrixService : IGrandPrixService
{
    
    private readonly IGrandPrixRepository _grandPrixRepository;

    public GrandPrixService(IGrandPrixRepository grandPrixRepository)
    {
        _grandPrixRepository = grandPrixRepository;
    }

    public async Task<IEnumerable<GrandPrix>> GetAll()
    {
        return await _grandPrixRepository.GetAll();
    }

    public async Task<List<string>> GetAllNames()
    {
        return await _grandPrixRepository.GetAllNames();
    }
}