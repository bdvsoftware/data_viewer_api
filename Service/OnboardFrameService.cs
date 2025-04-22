using DataViewerApi.Dto;
using DataViewerApi.Persistance.Entity;
using DataViewerApi.Persistance.Repository;

namespace DataViewerApi.Service;

public interface IOnboardFrameService
{
    public Task<OnboardFrame> SaveOnboardFrame(OnboardHelmetDto onboardHelmetData);
}

public class OnboardFrameService : IOnboardFrameService
{
    
    private readonly IOnboardFrameRepository _onboardFrameRepository;
    
    private readonly IFrameRepository _frameRepository;
    
    private readonly IDriverRepository _driverRepository;

    public OnboardFrameService(
        IOnboardFrameRepository onboardFrameRepository, 
        IFrameRepository frameRepository, 
        IDriverRepository driverRepository)
    {
        _onboardFrameRepository = onboardFrameRepository;
        _frameRepository = frameRepository;
        _driverRepository = driverRepository;
    }

    public async Task<OnboardFrame> SaveOnboardFrame(int frameId, OnboardHelmetDto onboardHelmetData)
    {
        var frame = await _frameRepository.GetFrameById(frameId);
        if (onboardHelmetData.Driver.Length > 3)
        {
            var driverName = await this.findBestMatchDriverName(onboardHelmetData.Driver);
            var driver = await _driverRepository.GetDriverByLowerCaseName(driverName);
        }
        else
        {
            var driver = await _driverRepository.GetDriverByAbbreviation(onboardHelmetData.Driver);
        }
    }

    private async Task<string?> findBestMatchDriverName(string driverName)
    {
        var names = await _driverRepository.GetDriversNames();
        var lowerNames = names.Select(n => n.ToLower());
        var bestMatchDriver = lowerNames.FirstOrDefault(n => n.ToLower().Contains(driverName.ToLower()));
        return bestMatchDriver;
    }
}