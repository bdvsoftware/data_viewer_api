namespace DataViewerApi.Dto;

public class DriverVideoDto
{
    public string Driver { get; set; }
    public DriverOnboardDto? DriverOnboardDto { get; set; }
    public DriverBatteryDto? DriverBatteryDto { get; set; }
}