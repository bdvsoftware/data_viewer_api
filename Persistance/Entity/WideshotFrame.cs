namespace DataViewerApi.Persistance.Entity;

public class WideshotFrame
{
    public int WideshotFrameId { get; set; }
    public int FrameId { get; set; }
    public DateTime Timestamp { get; set; }
    public string VideoUrl { get; set; }
    public double Duration { get; set; }

    public Frame Frame { get; set; }
    public ICollection<DriverWideshotFrame> DriverWideshotFrames { get; set; }
}
