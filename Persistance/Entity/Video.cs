using System;
using System.Collections.Generic;

namespace DataViewerApi.Prueba;

public partial class Video
{
    public int VideoId { get; set; }

    public int SessionId { get; set; }

    public string? Name { get; set; }

    public string Url { get; set; } = null!;

    public double Duration { get; set; }

    public int TotalFrames { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Frame> Frames { get; set; } = new List<Frame>();

    public virtual Session Session { get; set; } = null!;
}
