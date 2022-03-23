using Shared.Dtos.Responses;
using Shared.Dtos.Responses.Pagination;

namespace MetalServices.Contracts
{
    public interface IAlbumService
    {
        Task<AlbumResponse> GetAlbumAsync(Guid albumId);
        Task<AlbumResponse> GetRandomAsync();
        Task<PaginationResponse<AlbumResponse>> GetUpcomingAlbums(bool favorites);
    }
}
