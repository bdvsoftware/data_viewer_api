using System.Text.Json.Serialization;

namespace DataViewerApi.Dto;

public class ProcessedFrameDto
{
    [JsonPropertyName("video_id")]
    public int VideoId { get; set; }
    [JsonPropertyName("frame_id")]
    public int FrameId { get; set; }
    [JsonPropertyName("frame_seq")]
    public int FrameSeq { get; set; }
    [JsonPropertyName("frame_timestamp")]
    public int FrameTimestamp { get; set; }
    [JsonPropertyName("onboard_helmet_data")]
    public OnboardHelmetDto? OnboardHelmetDto { get; set; }
    [JsonPropertyName("battery_driver_data")]
    public BatteryDriverDataDto? BatteryDriverDataDto { get; set; }

    public ProcessedFrameDto(int videoId, int frameId, int frameSeq, int frameTimestamp, OnboardHelmetDto? onboardHelmetDto, BatteryDriverDataDto? batteryDriverDataDto)
    {
        VideoId = videoId;
        FrameId = frameId;
        FrameSeq = frameSeq;
        FrameTimestamp = frameTimestamp;
        OnboardHelmetDto = onboardHelmetDto;
        BatteryDriverDataDto = batteryDriverDataDto;
    }
}