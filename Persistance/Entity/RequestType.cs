namespace DataViewerApi.Persistance.Entity;

public class RequestType
{
    public int Id { get; set; }
    public int Name { get; set; }

    public RequestType(int name)
    {
        Name = name;
    }
}