namespace DataViewerApi.Dto;

public class FrameToProcessDto
{
    public int VideoId { get; set; }
    public int FrameId { get; set; }
    public int FrameSequence { get; set; }
    public int FrameTimestamp { get; set; }
    public string FrameImg { get; set; }

    public FrameToProcessDto(int videoId, int frameId, int frameSequence, int frameTimestamp, string frameImg)
    {
        VideoId = videoId;
        FrameId = frameId;
        FrameSequence = frameSequence;
        FrameTimestamp = frameTimestamp;
        FrameImg = frameImg;
    }
}