using AutoMapper;
using MetalModels;
using MetalServices.Contracts;
using MetalServices.Extensions;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.Responses;
using Shared.Dtos.Responses.Pagination;
using Shared.Exceptions;

namespace MetalServices
{
    public class AlbumService : IAlbumService
    {
        private readonly MetalDbContext _dbContext;
        private readonly IMapper _mapper;
        public AlbumService(MetalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<AlbumResponse> GetAlbumAsync(Guid albumId)
        {
            var album = await _dbContext.Albums
                .FirstOrDefaultAsync(b => b.AlbumId == albumId);

            if (album is null)
            {
                throw new NotFoundException("Album not found");
            }

            var albumResponse = _mapper.Map<AlbumResponse>(album);

            return albumResponse;
        }

        public async Task<AlbumResponse> GetRandomAsync()
        {
            var albums = _dbContext.Albums;

            var randomAlbum = await albums.Skip(new Random().Next(1, albums.Count())).Take(1).FirstOrDefaultAsync();

            var albumResponse = _mapper.Map<AlbumResponse>(randomAlbum);

            var band = await _dbContext.Bands
                .Include(b => b.Albums)
                .Include(b => b.BandGenres)
                    .ThenInclude(b => b.Genre)
                .FirstOrDefaultAsync(b => b.BandId == randomAlbum.BandId);

            albumResponse.Band = _mapper.Map<BandResponse>(band);

            return albumResponse;
        }

        public async Task<PaginationResponse<AlbumResponse>> GetUpcomingAlbums(bool favorites)
        {
            var albums = await _dbContext.Albums
                .Where(a => a.ReleaseDate.Contains("/"))
                .ToListAsync();

            var orderedAlbums = albums.Where(a => a.ReleaseDate.ParseDate() > DateTime.Today).OrderBy(a => a.ReleaseDate.ParseDate()).ToList();

            var albumResponse = _mapper.Map<IEnumerable<AlbumResponse>>(orderedAlbums);

            return new PaginationResponse<AlbumResponse>(albumResponse, albums.Count(), albums.Count(), 1, 100);
        }
    }
}
