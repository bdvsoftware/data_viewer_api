using DataViewerApi.Persistance.Entity;
using DataViewerApi.Persistance.Repository;

namespace DataViewerApi.Service;

public interface ISessionTypeService
{
    Task<IEnumerable<SessionType>> GetSessionTypes();
    Task<List<string>> GetAllNames();
}

public class SessionTypeService : ISessionTypeService
{
    private readonly ISessionTypeRepository _sessionTypeRepository;

    public SessionTypeService(ISessionTypeRepository sessionTypeRepository)
    {
        _sessionTypeRepository = sessionTypeRepository;
    }

    public async Task<IEnumerable<SessionType>> GetSessionTypes()
    {
        return await _sessionTypeRepository.GetAll();
    }

    public async Task<List<string>> GetAllNames()
    {
        return await _sessionTypeRepository.GetAllNames();
    }
}