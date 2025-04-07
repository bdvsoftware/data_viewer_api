namespace DataViewerApi.Persistance.Entity;

public class Driver
{
    public int DriverId { get; set; }
    public int TeamId { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }

    public Team Team { get; set; }
    public ICollection<OnboardFrame> OnboardFrames { get; set; }
    public ICollection<DrivereyeFrame> DrivereyeFrames { get; set; }
    public ICollection<DriverWideshotFrame> DriverWideshotFrames { get; set; }
    public ICollection<PitboostFrameDriver> PitboostFrameDrivers { get; set; }
}
