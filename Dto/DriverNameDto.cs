namespace DataViewerApi.Dto;

public class DriverNameDto
{
    public string DriverName { get; set; }
    public string DriverAbbreviation { get; set; }

    public DriverNameDto(string driverName, string driverAbbreviation)
    {
        DriverName = driverName;
        DriverAbbreviation = driverAbbreviation;
    }
}