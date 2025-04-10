using System;
using System.Collections.Generic;

namespace DataViewerApi.Prueba;

public partial class SessionType
{
    public int SessionTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}
