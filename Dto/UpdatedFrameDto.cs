using System.Text.Json.Serialization;

namespace DataViewerApi.Dto;

public class UpdatedFrameDto
{
    [JsonPropertyName("frameTimestamp")]
    public int FrameTimestamp { get; set; }
    [JsonPropertyName("videoId")]
    public int VideoId { get; set; }
    [JsonPropertyName("driverAbbr")]
    public string DriverAbbr { get; set; }
    [JsonPropertyName("lap")]
    public int Lap { get; set; }

    public UpdatedFrameDto(int frameTimestamp, int videoId, string driverAbbr, int lap)
    {
        FrameTimestamp = frameTimestamp;
        VideoId = videoId;
        DriverAbbr = driverAbbr;
        Lap = lap;
    }
}