using DataViewerApi.Dto;
using DataViewerApi.Exception;
using DataViewerApi.Persistance.Entity;
using DataViewerApi.Persistance.Repository;
using DataViewerApi.Utils;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DataViewerApi.Service;

public interface IOnboardHelmetFrameService
{
    public Task ProcessOnboardHelmetFrame(int frameId, OnboardHelmetDto onboardHelmetData);
}

public class OnboardHelmetFrameService : IOnboardHelmetFrameService
{
    private readonly IDrivereyeFrameRepository _drivereyeFrameRepository;

    private readonly IOnboardFrameRepository _onboardFrameRepository;

    private readonly IFrameRepository _frameRepository;

    private readonly IDriverRepository _driverRepository;

    public OnboardHelmetFrameService(IDrivereyeFrameRepository drivereyeFrameRepository,
        IOnboardFrameRepository onboardFrameRepository, IFrameRepository frameRepository,
        IDriverRepository driverRepository)
    {
        _drivereyeFrameRepository = drivereyeFrameRepository;
        _onboardFrameRepository = onboardFrameRepository;
        _frameRepository = frameRepository;
        _driverRepository = driverRepository;
    }

    public async Task ProcessOnboardHelmetFrame(int frameId, OnboardHelmetDto onboardHelmetData)
    {
        var camType = onboardHelmetData.Camera;
        var driver = await _driverRepository.GetDriverByAbbreviation(onboardHelmetData.DriverAbbreviation);
        await _frameRepository.UpdateFrameLap(frameId, onboardHelmetData.Lap);
        if (driver != null)
        {
            if (Constants.CameraType.Onboard.Equals(camType))
            {
                await SaveOnboardFrame(frameId, driver.DriverId);
            }
            else
            {
                await SaveHelmetFrame(frameId, driver.DriverId);
            }
        }
        else
        {
            throw new DriverNotFoundException();
        }
    }

    private async Task SaveOnboardFrame(int frameId, int driverId)
    {
        var onboardFrame = new OnboardFrame(
            frameId,
            driverId);
        await _onboardFrameRepository.AddOnboardFrame(onboardFrame);
    }

    private async Task SaveHelmetFrame(int frameId, int driverId)
    {
        var driverEyeFrame = new DrivereyeFrame(
            frameId,
            driverId);
        await _drivereyeFrameRepository.AddDrivereyeFrame(driverEyeFrame);
    }
}