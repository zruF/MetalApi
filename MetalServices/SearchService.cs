using AutoMapper;
using MetalModels;
using MetalModels.Contracts;
using MetalModels.Models;
using MetalServices.Contracts;
using MetalServices.Extensions;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.Requests;
using Shared.Dtos.Responses;

namespace MetalServices
{
    public class SearchService : ISearchService
    {
        private readonly MetalDbContext _dbContext;
        private readonly IMapper _mapper;

        public SearchService(MetalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SearchResponse>> GetEntities(SearchFilterRequest filter)
        {
            var entities = new List<SearchResponse>();

            IEnumerable<IEntity> bands, albums, genres;

            if(filter.EntityType == MetalModels.Types.EntityType.All || filter.EntityType == MetalModels.Types.EntityType.Band)
            {
                bands = await _dbContext.Bands.AsNoTracking().SetQueryFilter(filter).ToListAsync();
                entities.AddRange(_mapper.Map<IEnumerable<SearchResponse>>(bands));
            }
            if (filter.EntityType == MetalModels.Types.EntityType.All || filter.EntityType == MetalModels.Types.EntityType.Album)
            {
                albums = await _dbContext.Albums.AsNoTracking().SetQueryFilter(filter).ToListAsync();
                entities.AddRange(_mapper.Map<IEnumerable<SearchResponse>>(albums));
            }
            if (filter.EntityType == MetalModels.Types.EntityType.Genre)
            {
                genres = await _dbContext.Genres.Include(g => g.BandGenres).AsNoTracking().SetQueryFilter(filter).ToListAsync();
                var ordered = genres.Cast<Genre>().OrderByDescending(g => g.BandGenres.Count);
                entities.AddRange(_mapper.Map<IEnumerable<SearchResponse>>(ordered));
            }

            return entities;
        }
    }
}
