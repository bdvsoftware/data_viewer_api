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

    [HttpGet("all")]
    public async Task<IActionResult> GetAllVideos()
    {
        var videos = await _videoService.GetVideos();
        return Ok(videos);
    }

    [HttpPost("process/{videoId}")]
    public async Task ProcessVideo(int videoId)
    {
        await _videoService.StartVideoProcessing(videoId);
    }

    [HttpGet("data/{videoId}")]
    public async Task<IActionResult> GetVideoData(int videoId)
    {
        var data = await _videoService.GetVideoData(videoId);
        return Ok(data);
    }

    [HttpGet("download/{videoId}")]
    public async Task<IActionResult> DownloadVideo(int videoId)
    {
        var (stream, fileName) = await _videoService.GetVideoFile(videoId);
        return File(stream, "video/mp4", fileName);
    }

    [HttpGet("path/{videoId}")]
    public async Task<IActionResult> GetVideoPath(int videoId)
    {
        var path = await _videoService.GetVideoPath(videoId);
        return Ok(path);
    }
}