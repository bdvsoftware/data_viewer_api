namespace DataViewerApi.Dto;

public class VideoToProcessDto
{
    public int VideoId { get; set; }
    public string VideoUrl { get; set; }

    public VideoToProcessDto(int videoId, string videoUrl)
    {
        VideoId = videoId;
        VideoUrl = videoUrl;
    }
}