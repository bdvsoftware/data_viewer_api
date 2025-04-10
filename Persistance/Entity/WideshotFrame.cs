using System;
using System.Collections.Generic;

namespace DataViewerApi.Prueba;

public partial class WideshotFrame
{
    public int WideshotFrameId { get; set; }

    public int FrameId { get; set; }

    public DateTime Timestamp { get; set; }

    public string? VideoUrl { get; set; }

    public double Duration { get; set; }

    public virtual Frame Frame { get; set; } = null!;

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();
}
