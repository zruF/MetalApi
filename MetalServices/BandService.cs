using AutoMapper;
using MetalModels;
using MetalModels.Models;
using MetalServices.Contracts;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.Responses;
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

            var bandResponse = _mapper.Map<BandResponse>(band);

            if(band is null)
            {
                throw new NotFoundException("Band not found");
            }

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

        private async Task<Band> GetBandInternalAsync(Guid bandId)
        {
            return await _dbContext.Bands
                .Include(b => b.Albums)
                .Include(b => b.BandGenres)
                    .ThenInclude(bg => bg.Genre)
                .FirstOrDefaultAsync(b => b.BandId == bandId);
        }
    }
}
