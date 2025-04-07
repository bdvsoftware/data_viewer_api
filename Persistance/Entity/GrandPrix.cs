using System.ComponentModel.DataAnnotations;

namespace DataViewerApi.Persistance.Entity;

public class GrandPrix
{
    [Key]
    public int GpId { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public string Circuit { get; set; }

    public ICollection<Session> Sessions { get; set; }
}
