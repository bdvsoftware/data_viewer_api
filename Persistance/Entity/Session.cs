using System;
using System.Collections.Generic;

namespace DataViewerApi.Prueba;

public partial class Session
{
    public int SessionId { get; set; }

    public int GpId { get; set; }

    public int SessionTypeId { get; set; }

    public DateTime Date { get; set; }

    public virtual GrandPrix Gp { get; set; } = null!;

    public virtual SessionType SessionType { get; set; } = null!;

    public virtual Video? Video { get; set; }
}
