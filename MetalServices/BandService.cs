using MetalModels;
using MetalModels.Models;
using MetalServices.Contracts;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;

namespace MetalServices
{
    public class BandService : IBandService
    {
        private readonly MetalDbContext _dbContext;
        public BandService(MetalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Band> GetBandAsync(Guid bandId)
        {
            var band = await _dbContext.Bands.FirstOrDefaultAsync(b => b.BandId == bandId);

            if(band is null)
            {
                throw new NotFoundException("Band not found");
            }

            return band;
        }
    }
}
