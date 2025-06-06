﻿using System;
using System.Collections.Generic;

namespace DataViewerApi.Persistance.Entity;

public partial class PitboostFrame
{
    public int PitboostFrameId { get; set; }

    public int FrameId { get; set; }

    public int? Status { get; set; }

    public virtual Frame Frame { get; set; } = null!;

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();
}
