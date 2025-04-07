namespace DataViewerApi.Persistance.Entity;

public class DriverWideshotFrame
{
    public int DriverId { get; set; }
    public int WideshotFrameId { get; set; }

    public Driver Driver { get; set; }
    public WideshotFrame WideshotFrame { get; set; }

    public int DriverWideshotFrameId { get; set; } 
}

