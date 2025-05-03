namespace DataViewerApi.Persistance.Entity;

public partial class BatteryFrame
{
    public int BatteryFrameId { get; set; }
    public int FrameId { get; set; }
    
    public Frame Frame { get; set; }
    public ICollection<BatteryFrameDriver> BatteryFrameDrivers { get; set; }

    public BatteryFrame(int frameId)
    {
        FrameId = frameId;
    }
}