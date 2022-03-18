using MetalModels;
using MetalModels.Models;
using MetalServices.Contracts;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;

namespace MetalServices
{
    public class AlbumService : IAlbumService
    {
        private readonly MetalDbContext _dbContext;
        public AlbumService(MetalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Album> GetAlbumAsync(Guid albumId)
        {
            var album = await _dbContext.Albums
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(b => b.AlbumId == albumId);

            if (album is null)
            {
                throw new NotFoundException("Album not found");
            }

            return album;
        }
    }
}
