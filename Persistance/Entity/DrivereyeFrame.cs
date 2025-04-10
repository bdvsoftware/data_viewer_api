using System;
using System.Collections.Generic;

namespace DataViewerApi.Prueba;

public partial class DrivereyeFrame
{
    public int DrivereyeFrameId { get; set; }

    public int FrameId { get; set; }

    public int? DriverId { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual Driver? Driver { get; set; }

    public virtual Frame Frame { get; set; } = null!;
}
