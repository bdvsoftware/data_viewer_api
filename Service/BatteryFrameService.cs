﻿using DataViewerApi.Dto;
using DataViewerApi.Exception;
using DataViewerApi.Persistance.Entity;
using DataViewerApi.Persistance.Repository;

namespace DataViewerApi.Service;

public interface IBatteryFrameService
{
    public Task ProcessBatteryFrame(int frameId, BatteryDriverDataDto batteryData);
}

public class BatteryFrameService : BaseService, IBatteryFrameService
{

    private readonly IDriverRepository _driverRepository;
    
    private readonly IBatteryFrameRepository _batteryFrameRepository;
    
    private readonly IBatteryFrameDriverRepository _batteryFrameDriverRepository;
    
    private readonly IFrameRepository _frameRepository;
    
    public BatteryFrameService(IDriverRepository driverRepository, IBatteryFrameRepository batteryFrameRepository, IBatteryFrameDriverRepository batteryFrameDriverRepository, IFrameRepository frameRepository)
    {
        _driverRepository = driverRepository;
        _batteryFrameRepository = batteryFrameRepository;
        _batteryFrameDriverRepository = batteryFrameDriverRepository;
        _frameRepository = frameRepository;
    }

    public async Task ProcessBatteryFrame(int frameId, BatteryDriverDataDto batteryData)
    {
        var batteryFrame = new BatteryFrame(
            frameId
        );
        var lap = ExtractLapNumber(batteryData.Lap);
        await _frameRepository.UpdateFrameLap(frameId, lap);
        var savedBatteryFrame = await _batteryFrameRepository.AddBatteryFrame(batteryFrame);
        foreach (var key in batteryData.Battery.Keys)
        {
            if (key.ToLower() != "lap")
            {
                var driverAbbreviation = key;
                try
                {
                    var driver = await _driverRepository.GetDriverByAbbreviation(driverAbbreviation);
                    
                    if (driver == null)
                    {
                        throw new DriverNotFoundException();
                    }

                    var batteryStatus = batteryData.Battery[key];
                    var batteryFrameDriver = new BatteryFrameDriver(
                        savedBatteryFrame.BatteryFrameId,
                        driver.DriverId,
                        batteryStatus
                    );
                    await _batteryFrameDriverRepository.AddBatteryFrameDriver(batteryFrameDriver);
                }
                catch (DriverNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
        }
    }
}