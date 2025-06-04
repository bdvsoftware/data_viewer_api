namespace DataViewerApi.Dto;

public class UpdateFrameRequestDto
{ 
    public int? VideoId { get; set; }
    public int InitTime { get; set; }
    public int EndTime { get; set; }
    public int? Lap { get; set; }
    public string DriverAbbr { get; set; }

    public UpdateFrameRequestDto(int? videoId, int initTime, int endTime, int? lap, string driverAbbr)
    {
        VideoId = videoId;
        InitTime = initTime;
        EndTime = endTime;  
        Lap = lap;
        DriverAbbr = driverAbbr;
    }
}