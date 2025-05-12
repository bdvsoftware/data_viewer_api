namespace DataViewerApi.Dto;

public class VideoToProcessDto
{
    public int VideoId { get; set; }
    public string VideoUrl { get; set; }
    
    public int Threshold { get; set; }

    public VideoToProcessDto(int videoId, string videoUrl, int threshold)
    {
        VideoId = videoId;
        VideoUrl = videoUrl;
        Threshold = threshold;
    }
}