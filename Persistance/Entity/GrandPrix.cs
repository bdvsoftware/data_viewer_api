using System;
using System.Collections.Generic;

namespace DataViewerApi.Persistance.Entity;

public partial class GrandPrix
{
    public int GpId { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly Date { get; set; }

    public string Circuit { get; set; } = null!;

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}
