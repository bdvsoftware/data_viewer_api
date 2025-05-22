using System.Text.Json.Serialization;

namespace DataViewerApi.Dto;

public class RequestUploadVideoDto
{
    public string GrandPrixName { get; set; }
    public string SessionName { get; set; }
    public IFormFile File { get; set; }
    public String Path { get; set; }
}