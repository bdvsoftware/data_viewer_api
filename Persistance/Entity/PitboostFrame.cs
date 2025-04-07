using System.ComponentModel.DataAnnotations;

namespace DataViewerApi.Persistance.Entity;

public class PitboostFrame
{
    [Key]
    public int PitboostFrameId { get; set; }
    public int FrameId { get; set; }
    public int? Status { get; set; }
    public DateTime Timestamp { get; set; }

    public Frame Frame { get; set; }
    public ICollection<PitboostFrameDriver> PitboostFrameDrivers { get; set; }
}
