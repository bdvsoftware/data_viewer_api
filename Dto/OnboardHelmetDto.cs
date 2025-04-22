using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataViewerApi.Dto;

public class OnboardHelmetDto
{
    [JsonPropertyName("driver")]
    public string Driver { get; set; }
    [JsonPropertyName("team")]
    public string Team { get; set; }
    [JsonPropertyName("cam")]
    public string Camera { get; set; }

    public OnboardHelmetDto(string driver, string team, string camera)
    {
        Driver = driver;
        Team = team;
        Camera = camera;
    }
}