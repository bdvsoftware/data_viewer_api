namespace DataViewerApi.Persistance.Entity;

public class SessionType
{
    public int SessionTypeId { get; set; }
    public string Name { get; set; }

    public ICollection<Session> Sessions { get; set; }
}
