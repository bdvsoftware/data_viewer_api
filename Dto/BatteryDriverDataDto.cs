using System.Text.Json.Serialization;

namespace DataViewerApi.Dto;

public class BatteryDriverDataDto
{
    [JsonPropertyName("battery")]
    public Dictionary<string, int> Battery { get; set; }

    [JsonPropertyName("lap")]
    public int Lap { get; set; }

    public BatteryDriverDataDto(Dictionary<string, int> battery, int lap)
    {
        Battery = battery;
        Lap = lap;
    }
}