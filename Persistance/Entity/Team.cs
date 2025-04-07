namespace DataViewerApi.Persistance.Entity;

public class Team
{
    public int TeamId { get; set; }
    public string Name { get; set; }

    public ICollection<Driver> Drivers { get; set; }
}
