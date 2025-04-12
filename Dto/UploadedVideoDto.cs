using System.Text.Json.Serialization;

namespace DataViewerApi.Dto;

public class UploadedVideoDto
{
    [JsonPropertyName("videoId")]
    public int VideoId { get; set; }
    [JsonPropertyName("videoName")]
    public string VideoName { get; set; }
    [JsonPropertyName("videoPath")]
    public string VideoPath { get; set; }

    public UploadedVideoDto(int videoId, string videoName, string videoPath)
    {
        VideoId = videoId;
        VideoName = videoName;
        VideoPath = videoPath;
    }
}