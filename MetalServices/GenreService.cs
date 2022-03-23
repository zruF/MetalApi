using AutoMapper;
using MetalModels;
using MetalModels.Models;
using MetalServices.Contracts;
using MetalServices.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MetalServices
{
    public class GenreService : IGenreService
    {
        private readonly MetalDbContext _dbContext;
        private readonly IMapper _mapper;

        public GenreService(MetalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<string>> GetCommonGenres()
        {
            var genres = await _dbContext.Genres.AsNoTracking().ToListAsync();

            var commonGenres = genres
                        .Select(x => x.Name.GetParentGenre())
                        .GroupBy(x => x)
                        .Select(g => new
                        {
                            Genre = g.Key,
                            Count = g.Count()
                        })
                        .OrderByDescending(g => g.Count)
                        .Select(x => x.Genre)
                        .Take(10).ToList();

            return commonGenres;
        }
    }
}
