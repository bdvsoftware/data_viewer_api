using DataViewerApi.Persistance.Entity;
using DataViewerApi.Persistance.Repository;

namespace DataViewerApi.Service;

public interface ISessionService
{
    
}

public class SessionService : ISessionService
{
    private readonly ISessionTypeRepository _sessionTypeRepository;

    public SessionService(ISessionTypeRepository sessionTypeRepository)
    {
        _sessionTypeRepository = sessionTypeRepository;
    }
}