using DataViewerApi.Persistance.Repository;
using DataViewerApi.Persistance.Entity;
using DataViewerApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace DataViewerApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class GrandPrixController : ControllerBase
{
    private readonly IGrandPrixService _grandPrixService;

    public GrandPrixController(IGrandPrixService grandPrixService)
    {
        _grandPrixService = grandPrixService;
    }

    [HttpGet("all")]
    public async Task<IEnumerable<GrandPrix>> GetAllGrandPrix()
    {
        return await _grandPrixService.GetAll();
    }
    
    [HttpGet("all-names")]
    public async Task<List<string>> GetAllGrandPrixNames()
    {
        return await _grandPrixService.GetAllNames();
    }
}