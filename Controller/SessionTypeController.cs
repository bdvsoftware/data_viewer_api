using DataViewerApi.Persistance.Entity;
using DataViewerApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace DataViewerApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class SessionTypeController : ControllerBase
{
    private readonly ISessionTypeService _sessionTypeService;

    public SessionTypeController(ISessionTypeService sessionTypeService)
    {
        _sessionTypeService = sessionTypeService;
    }

    [HttpGet("all")]
    public async Task<IEnumerable<SessionType>> GetSessionTypes()
    {
        return await _sessionTypeService.GetSessionTypes();
    }
}