using System.ComponentModel.DataAnnotations;

namespace DataViewerApi.Persistance.Entity;

public class Session
{
    [Key]
    public int SessionId { get; set; }
    public int GpId { get; set; }
    public int SessionTypeId { get; set; }
    public DateTime Date { get; set; }

    public GrandPrix GrandPrix { get; set; }
    public SessionType SessionType { get; set; }
    public Video Video { get; set; }
}
