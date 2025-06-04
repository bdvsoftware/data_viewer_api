namespace DataViewerApi.Dto;

public class UpdateFrameRequestDto
{ 
    public int VideoId { get; set; }
    public int Timestamp { get; set; }
    public int Lap { get; set; }
    public string DriverAbbr { get; set; }
    
    public UpdateFrameRequestDto(int videoId, int timestamp, int lap, string driverAbbr)
    {
        VideoId = videoId;
        Timestamp = timestamp;
        Lap = lap;
        DriverAbbr = driverAbbr;
    }
}