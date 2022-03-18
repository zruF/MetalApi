using MetalModels.Models;

namespace MetalServices.Contracts
{
    public interface IBandService
    {
        public Task<Band> GetBandAsync(Guid bandId);
    }
}
