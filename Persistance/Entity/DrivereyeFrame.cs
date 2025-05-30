﻿using System;
using System.Collections.Generic;

namespace DataViewerApi.Persistance.Entity;

public partial class DrivereyeFrame
{
    public int DrivereyeFrameId { get; set; }

    public int FrameId { get; set; }

    public int? DriverId { get; set; }

    public virtual Driver? Driver { get; set; }

    public virtual Frame Frame { get; set; } = null!;

    public DrivereyeFrame(int frameId, int? driverId)
    {
        FrameId = frameId;
        DriverId = driverId;
    }
}
