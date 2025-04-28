namespace DataViewerApi.Dto;

public class DriverVideoDto
{
    public IEnumerable<DriverOnboardDto>? DriverOnboardDto { get; set; }
    public IEnumerable<DriverBatteryDto>? DriverBatteryDto { get; set; }

    public DriverVideoDto(IEnumerable<DriverOnboardDto>? driverOnboardDto, IEnumerable<DriverBatteryDto>? driverBatteryDto)
    {
        DriverOnboardDto = driverOnboardDto;
        DriverBatteryDto = driverBatteryDto;
    }
}