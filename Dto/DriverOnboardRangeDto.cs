namespace DataViewerApi.Dto;

public class DriverOnboardRangeDto
{
    public string DriverName { get; set; }
    public string DriverAbbreviation { get; set; }
    public string TeamName { get; set; }
    public int OnboardFrameId { get; set; }
    public string TimeRange { get; set; }
    public int? Lap { get; set; }

    public DriverOnboardRangeDto(string driverName, string driverAbbreviation, string teamName, int onboardFrameId, string timeRange, int? lap)
    {
        DriverName = driverName;
        DriverAbbreviation = driverAbbreviation;
        TeamName = teamName;
        OnboardFrameId = onboardFrameId;
        TimeRange = timeRange;
        Lap = lap;
    }
}