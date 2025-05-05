namespace DataViewerApi.Dto;

public class BatteryFrameDriverIdDto
{
    public int DriverId { get; set; }
    public int BatteryFrameId { get; set; }

    public BatteryFrameDriverIdDto(int driverId, int batteryFrameId)
    {
        DriverId = driverId;
        BatteryFrameId = batteryFrameId;
    }
}