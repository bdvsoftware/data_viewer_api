using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataViewerApi.Persistance.Entity;

public partial class Video
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int VideoId { get; set; }

    public int SessionId { get; set; }

    public string Name { get; set; }

    public string Url { get; set; } = null!;

    public double  Duration { get; set; }

    public int TotalFrames { get; set; }
    
    public int ProcessedFrames { get; set; }

    public string Status { get; set; } = null!;
    
    public int FrameRate { get; set; }

    public virtual ICollection<Frame> Frames { get; set; } = new List<Frame>();

    public virtual Session Session { get; set; } = null!;

    public Video(int sessionId, string name, string url, double duration, int totalFrames, string status)
    {
        SessionId = sessionId;
        Name = name;
        Url = url;
        Duration = duration;
        TotalFrames = totalFrames;
        ProcessedFrames = 0;
        Status = status;
    }
}
