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

    public Task UpdateOnboardFrameData(int frameId, string driverAbbreviation);
}

public class OnboardHelmetFrameService : BaseService, IOnboardHelmetFrameService
{
    private readonly IDrivereyeFrameRepository _drivereyeFrameRepository;

    private readonly IOnboardFrameRepository _onboardFrameRepository;

    private readonly IFrameRepository _frameRepository;

    private readonly IDriverRepository _driverRepository;
    
    private readonly IVideoRepository _videoRepository;

    public OnboardHelmetFrameService(IDrivereyeFrameRepository drivereyeFrameRepository, IOnboardFrameRepository onboardFrameRepository, IFrameRepository frameRepository, IDriverRepository driverRepository, IVideoRepository videoRepository)
    {
        _drivereyeFrameRepository = drivereyeFrameRepository;
        _onboardFrameRepository = onboardFrameRepository;
        _frameRepository = frameRepository;
        _driverRepository = driverRepository;
        _videoRepository = videoRepository;
    }

    public async Task ProcessOnboardHelmetFrame(int frameId, OnboardHelmetDto onboardHelmetData)
    {
        var camType = onboardHelmetData.Camera;
        var driver = await _driverRepository.GetDriverByAbbreviation(onboardHelmetData.DriverAbbreviation);
        var lap = ExtractLapNumber(onboardHelmetData.Lap);
        await _frameRepository.UpdateFrameLap(frameId, lap);
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

    public async Task UpdateOnboardFrameData(int frameId, string driverAbbreviation)
    {
        var driver = await _driverRepository.GetDriverByAbbreviation(driverAbbreviation);
        if (driver == null)
        {
            throw new DriverNotFoundException();
        }
        else
        {
            var driverId = driver.DriverId;
            await _onboardFrameRepository.UpdateOnboardFrameDriverByVideoIdTimestamp(frameId, driverId);
        }
    }
}