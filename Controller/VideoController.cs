using DataViewerApi.Dto;
using DataViewerApi.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DataViewerApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class VideoController
{
    
    private readonly IVideoService _videoService;

    public VideoController(IVideoService videoService)
    {
        _videoService = videoService;
    }

    [HttpPost]
    public async Task<Boolean> ExistsVideo([FromBody] RequestFrameDto request)
    {
        var exists = await _videoService.ExistsVideoByName(request.VideoName);
        return exists;
    }
}