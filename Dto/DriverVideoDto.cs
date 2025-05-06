namespace DataViewerApi.Dto;

public class DriverVideoDto
{
    public IEnumerable<DriverOnboardRangeDto>? DriverOnboardRangeDto { get; set; }
    public IEnumerable<DriverBatteryRangeDto>? DriverBatteryRangeDto { get; set; }

    public DriverVideoDto(IEnumerable<DriverOnboardRangeDto>? driverOnboardRangeDto, IEnumerable<DriverBatteryRangeDto>? driverBatteryRangeDto)
    {
        DriverOnboardRangeDto = driverOnboardRangeDto;
        DriverBatteryRangeDto = driverBatteryRangeDto;
    }
}