using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataViewerApi.Dto;

public class OnboardHelmetDto
{
    [JsonPropertyName("driver_name")]
    public string Driver { get; set; }
    [JsonPropertyName("driver_abbr")]
    public string DriverAbbreviation { get; set; }
    [JsonPropertyName("cam")]
    public string Camera { get; set; }
    [JsonPropertyName("lap")]
    public string Lap { get; set; }

    public OnboardHelmetDto(string driver, string driverAbbreviation, string camera, string lap)
    {
        Driver = driver;
        DriverAbbreviation = driverAbbreviation;
        Camera = camera;
        Lap = lap;
    }
}