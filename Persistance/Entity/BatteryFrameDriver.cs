namespace DataViewerApi.Persistance.Entity;

public class BatteryFrameDriver
{
    public int BatteryFrameId { get; set; }
    public int DriverId { get; set; }
    public int Status { get; set; }

    public BatteryFrame BatteryFrame { get; set; }
    public Driver Driver { get; set; }
}