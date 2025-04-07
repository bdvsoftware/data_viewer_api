namespace DataViewerApi.Persistance.Entity;

public class Frame
{
    public int FrameId { get; set; }
    public int VideoId { get; set; }
    public DateTime Timestamp { get; set; }

    public Video Video { get; set; }
    public ICollection<OnboardFrame> OnboardFrames { get; set; }
    public ICollection<DrivereyeFrame> DrivereyeFrames { get; set; }
    public ICollection<PitboostFrame> PitboostFrames { get; set; }
    public ICollection<WideshotFrame> WideshotFrames { get; set; }
}
