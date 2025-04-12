using DataViewerApi.Persistance.Entity;
using DataViewerApi.Persistance.Repository;

namespace DataViewerApi.Service;

public interface ISessionTypeService
{
    Task<IEnumerable<SessionType>> GetSessionTypes();
}

public class SessionTypeService
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
}