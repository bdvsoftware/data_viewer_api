using System;
using System.Collections.Generic;

namespace DataViewerApi.Prueba;

public partial class Frame
{
    public int FrameId { get; set; }

    public int VideoId { get; set; }

    public int Seq { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual ICollection<DrivereyeFrame> DrivereyeFrames { get; set; } = new List<DrivereyeFrame>();

    public virtual ICollection<OnboardFrame> OnboardFrames { get; set; } = new List<OnboardFrame>();

    public virtual ICollection<PitboostFrame> PitboostFrames { get; set; } = new List<PitboostFrame>();

    public virtual Video Video { get; set; } = null!;

    public virtual ICollection<WideshotFrame> WideshotFrames { get; set; } = new List<WideshotFrame>();
}
