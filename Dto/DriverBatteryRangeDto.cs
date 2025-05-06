namespace DataViewerApi.Dto;

public class DriverBatteryRangeDto
{
    public string DriverName { get; set; }
    public string DriverAbbreviation { get; set; }
    public string TimeRange { get; set; }
    public int FrameId { get; set; }
    public int BatteryFrameId { get; set; }
    public int? Lap { get; set; }
    public int Status { get; set; }

    public DriverBatteryRangeDto(string driverName, string driverAbbreviation, string timeRange, int frameId, int batteryFrameId, int? lap, int status)
    {
        DriverName = driverName;
        DriverAbbreviation = driverAbbreviation;
        TimeRange = timeRange;
        FrameId = frameId;
        BatteryFrameId = batteryFrameId;
        Lap = lap;
        Status = status;
    }
}