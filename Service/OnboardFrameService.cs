using DataViewerApi.Dto;
using DataViewerApi.Persistance.Entity;

namespace DataViewerApi.Service;

public interface IOnboardFrameService
{
    public Task<OnboardFrame> SaveOnboardFrame(OnboardHelmetDto data);
}

public class OnboardFrameService : IOnboardFrameService
{
    
    private readonly OnboardFrameRepo
    
    public Task<OnboardFrame> SaveOnboardFrame(OnboardHelmetDto data)
    {
        
    }
}