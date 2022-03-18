using MetalModels.Models;

namespace MetalServices.Contracts
{
    public interface IAlbumService
    {
        Task<Album> GetAlbumAsync(Guid albumId);
    }
}
