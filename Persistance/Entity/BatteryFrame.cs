namespace DataViewerApi.Persistance.Entity;

public partial class BatteryFrame
{
    public int BatteryFrameId { get; set; }
    public int FrameId { get; set; }
    public int Lap { get; set; }
    
    public Frame Frame { get; set; }
    public ICollection<BatteryFrameDriver> BatteryFrameDrivers { get; set; }

    public BatteryFrame(int frameId, int lap)
    {
        FrameId = frameId;
        Lap = lap;
    }
}