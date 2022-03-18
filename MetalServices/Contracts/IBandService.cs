using Shared.Dtos.Responses;

namespace MetalServices.Contracts
{
    public interface IBandService
    {
        public Task<BandResponse> GetBandAsync(Guid bandId);
    }
}
