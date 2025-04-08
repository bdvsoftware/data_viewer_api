namespace DataViewerApi.Dto;

public class RequestFrameDto
{
    public string VideoName { get; set; }
    public string VideoPath { get; set; }
    public string Timestamp { get; set; }
    public int Sequence { get; set; }
    public string Base64Image { get; set; }
}