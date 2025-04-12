using DataViewerApi.Dto;
using DataViewerApi.Service;
using DataViewerApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DataViewerApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class VideoController : ControllerBase
{
    
    private readonly IVideoService _videoService;

    public VideoController(IVideoService videoService)
    {
        _videoService = videoService;
        if (!Directory.Exists(Constants.VideoDirectory))
        {
            Directory.CreateDirectory(Constants.VideoDirectory);
        }
    }

    [HttpPost]
    public async Task<Boolean> ExistsVideo([FromBody] RequestFrameDto request)
    {
        var exists = await _videoService.ExistsVideoByName(request.VideoName);
        return exists;
    }

    
    [HttpPost("upload")]
    public async Task<ActionResult<UploadedVideoDto>> UploadVideo([FromForm] RequestUploadVideoDto request)
    {

        var response = await _videoService.UploadVideo(request);

        return Ok(response);
    }
}