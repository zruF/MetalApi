using AutoMapper;
using MetalModels;
using MetalModels.Models;
using MetalServices.Contracts;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.Requests;
using Shared.Dtos.Responses;
using Shared.Dtos.Responses.Pagination;
using Shared.Exceptions;

namespace MetalServices
{
    public class BandService : IBandService
    {
        private readonly MetalDbContext _dbContext;
        private readonly IMapper _mapper;
        public BandService(MetalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<BandResponse> GetBandAsync(Guid bandId)
        {
            var band = GetBandInternalAsync(bandId);

            if (band is null)
            {
                throw new NotFoundException("Band not found");
            }

            var bandResponse = _mapper.Map<BandResponse>(band);
            return bandResponse;
        }

        public async Task<BandResponse> SetFavoriteAsync(Guid bandId)
        {
            var band = await GetBandInternalAsync(bandId);
            await _dbContext.SaveChangesAsync();
            var bandResponse = _mapper.Map<BandResponse>(band);
            return bandResponse;
        }

        public async Task<BandResponse> SetUnfavoriteAsync(Guid bandId)
        {
            var band = await GetBandInternalAsync(bandId);
            await _dbContext.SaveChangesAsync();
            var bandResponse = _mapper.Map<BandResponse>(band);
            return bandResponse;
        }

        public async Task<BandResponse> GetRandomAsync()
        {
            var bands = _dbContext.Bands
                .Include(b => b.Albums)
                .Include(b => b.BandGenres)
                    .ThenInclude(bg => bg.Genre);

            var randomBand = await bands.Skip(new Random().Next(1, bands.Count())).Take(1).FirstOrDefaultAsync();

            var bandResponse = _mapper.Map<BandResponse>(randomBand);

            return bandResponse;
        }

        private async Task<Band> GetBandInternalAsync(Guid bandId)
        {
            return await _dbContext.Bands
                .Include(b => b.Albums)
                .Include(b => b.BandGenres)
                    .ThenInclude(bg => bg.Genre)
                .FirstOrDefaultAsync(b => b.BandId == bandId);
        }

        public async Task<PaginationResponse<BandResponse>> GetBandsByGenre(PaginationRequest request)
        {
            var bands = _dbContext.Bands
                .Include(b => b.Albums)
                .Include(b => b.BandGenres)
                    .ThenInclude(b => b.Genre);

            var filteredBands = await bands
                .Where(b => b.BandGenres.Any(bg => bg.Genre.Name == request.Filter.Name))
                .Skip(Math.Max(0, request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var bandResponse = _mapper.Map<IEnumerable<BandResponse>>(filteredBands);

            return new PaginationResponse<BandResponse>(bandResponse, bandResponse.Count(), bands.Count(), request.PageSize, request.PageIndex);
        }
    }
}
