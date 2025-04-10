using System;
using System.Collections.Generic;

namespace DataViewerApi.Prueba;

public partial class Team
{
    public int TeamId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();
}
