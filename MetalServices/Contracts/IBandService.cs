using Shared.Dtos.Responses;

namespace MetalServices.Contracts
{
    public interface IBandService
    {
        Task<BandResponse> GetBandAsync(Guid bandId);
        Task<BandResponse> SetFavoriteAsync(Guid bandId);
        Task<BandResponse> SetUnfavoriteAsync(Guid bandId);
    }
}
