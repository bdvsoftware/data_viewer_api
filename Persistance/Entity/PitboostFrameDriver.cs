namespace DataViewerApi.Persistance.Entity;

public class PitboostFrameDriver
{
    public int PitboostFrameId { get; set; }
    public int DriverId { get; set; }

    public PitboostFrame PitboostFrame { get; set; }
    public Driver Driver { get; set; }
    
    public int PitboosFrameDriverId { get; set; }
}
