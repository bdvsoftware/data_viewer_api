using System.Text.Json.Serialization;

namespace DataViewerApi.Dto;

public class BatteryDriverDataDto
{
    [JsonPropertyName("battery")]
    public Dictionary<string, int> Battery { get; set; }

    [JsonPropertyName("lap")]
    public string Lap { get; set; }

    public BatteryDriverDataDto(Dictionary<string, int> battery, string lap)
    {
        Battery = battery;
        Lap = lap;
    }
}