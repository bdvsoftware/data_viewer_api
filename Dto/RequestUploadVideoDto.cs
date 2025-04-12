using System.Text.Json.Serialization;

namespace DataViewerApi.Dto;

public class RequestUploadVideoDto
{
    [JsonPropertyName("grandPrixName")]
    public string GrandPrixName { get; set; }
    [JsonPropertyName("sessionName")]
    public string SessionName { get; set; }
    [JsonPropertyName("file")]
    public IFormFile File { get; set; }
}