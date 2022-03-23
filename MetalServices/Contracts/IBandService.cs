using Shared.Dtos.Contracts;
using Shared.Dtos.Requests;
using Shared.Dtos.Responses;
using Shared.Dtos.Responses.Pagination;

namespace MetalServices.Contracts
{
    public interface IBandService
    {
        Task<BandResponse> GetBandAsync(Guid bandId);
        Task<BandResponse> SetFavoriteAsync(Guid bandId);
        Task<BandResponse> SetUnfavoriteAsync(Guid bandId);
        Task<BandResponse> GetRandomAsync();
        Task<PaginationResponse<BandResponse>> GetBandsByGenre(PaginationRequest request);
    }
}
