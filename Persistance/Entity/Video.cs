using System.ComponentModel.DataAnnotations;

namespace DataViewerApi.Persistance.Entity;

public class Video
{
    [Key]
    public int VideoId { get; set; }
    public int SessionId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public double Duration { get; set; }

    public Session Session { get; set; }
    public ICollection<Frame> Frames { get; set; }
}
