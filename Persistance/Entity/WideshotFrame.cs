using System;
using System.Collections.Generic;

namespace DataViewerApi.Persistance.Entity;

public partial class WideshotFrame
{
    public int WideshotFrameId { get; set; }

    public int FrameId { get; set; }

    public virtual Frame Frame { get; set; } = null!;

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();
}
