namespace DataViewerApi.Persistance.Entity;

public class DrivereyeFrame
{
    public int DrivereyeFrameId { get; set; }
    public int FrameId { get; set; }
    public int DriverId { get; set; }
    public DateTime Timestamp { get; set; }

    public Frame Frame { get; set; }
    public Driver Driver { get; set; }
}
