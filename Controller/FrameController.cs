using DataViewerApi.Dto;
using DataViewerApi.Exception;
using DataViewerApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace DataViewerApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class FrameController : ControllerBase
{
    private readonly IFrameService _frameService;
    
    public FrameController(IFrameService frameService)
    {
        _frameService = frameService;
    }

    [HttpPost("update")]
    public async Task<ActionResult> UpdateFrameData([FromBody] UpdateFrameRequestDto request)
    {
        try
        {
            await _frameService.UpdateFrameData(
                request.VideoId,
                request.Timestamp,
                request.Lap,
                request.DriverAbbr);
            return Ok();
        }
        catch (FrameNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}