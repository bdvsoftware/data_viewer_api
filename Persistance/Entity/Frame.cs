using System;
using System.Collections.Generic;

namespace DataViewerApi.Persistance.Entity;

public partial class Frame
{
    public int FrameId { get; set; }

    public int VideoId { get; set; }

    public int Seq { get; set; }

    public int Timestamp { get; set; }

    public Frame(int videoId, int seq, int timestamp)
    {
        VideoId = videoId;
        Seq = seq;
        Timestamp = timestamp;
    }

    public virtual ICollection<DrivereyeFrame> DrivereyeFrames { get; set; } = new List<DrivereyeFrame>();

    public virtual ICollection<OnboardFrame> OnboardFrames { get; set; } = new List<OnboardFrame>();

    public virtual ICollection<PitboostFrame> PitboostFrames { get; set; } = new List<PitboostFrame>();

    public virtual Video Video { get; set; } = null!;

    public virtual ICollection<WideshotFrame> WideshotFrames { get; set; } = new List<WideshotFrame>();
}
