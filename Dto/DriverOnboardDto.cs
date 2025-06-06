﻿namespace DataViewerApi.Dto;

public class DriverOnboardDto
{
    public string DriverName { get; set; }
    public string DriverAbbreviation { get; set; }
    public string TeamName { get; set; }
    public int OnboardFrameId { get; set; }
    public int Timestamp { get; set; }
    public int? Lap { get; set; }

    public DriverOnboardDto(string driverName, string driverAbbreviation, string teamName, int onboardFrameId, int timestamp, int? lap)
    {
        DriverName = driverName;
        DriverAbbreviation = driverAbbreviation;
        TeamName = teamName;
        OnboardFrameId = onboardFrameId;
        Timestamp = timestamp;
        Lap = lap;
    }
}