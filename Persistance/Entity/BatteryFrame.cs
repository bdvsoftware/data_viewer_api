namespace DataViewerApi.Persistance.Entity;

public partial class BatteryFrame
{
    public int BatteryFrameId { get; set; }
    public int FrameId { get; set; }
    public int Lap { get; set; }
    
    public virtual Frame Frame { get; set; } = null!;

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();

    public BatteryFrame(int frameId, int lap)
    {
        FrameId = frameId;
        Lap = lap;
    }
}