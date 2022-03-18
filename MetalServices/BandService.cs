using AutoMapper;
using MetalModels;
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
            var band = await _dbContext.Bands
                .Include(b => b.Albums)
                    .ThenInclude(a => a.Songs)
                .Include(b => b.BandGenres)
                    .ThenInclude(bg => bg.Genre)
                .FirstOrDefaultAsync(b => b.BandId == bandId);

            var bandResponse = _mapper.Map<BandResponse>(band);

            if(band is null)
            {
                throw new NotFoundException("Band not found");
            }

            return bandResponse;
        }
    }
}
