﻿using System;
using System.Collections.Generic;

namespace DataViewerApi.Persistance.Entity;

public partial class Driver
{
    public int DriverId { get; set; }

    public int TeamId { get; set; }

    public string Name { get; set; } = null!;

    public int Number { get; set; }
    
    public string Abbreviation { get; set; } = null!;

    public virtual ICollection<DrivereyeFrame> DrivereyeFrames { get; set; } = new List<DrivereyeFrame>();

    public virtual ICollection<OnboardFrame> OnboardFrames { get; set; } = new List<OnboardFrame>();

    public virtual Team Team { get; set; } = null!;

    public virtual ICollection<PitboostFrame> PitboostFrames { get; set; } = new List<PitboostFrame>();

    public virtual ICollection<BatteryFrameDriver> BatteryFrameDrivers { get; set; } = new List<BatteryFrameDriver>();

    public virtual ICollection<WideshotFrame> WideshotFrames { get; set; } = new List<WideshotFrame>();
}
