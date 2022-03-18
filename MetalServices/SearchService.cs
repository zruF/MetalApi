using AutoMapper;
using MetalModels;
using MetalServices.Contracts;
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

            var bands = await _dbContext.Bands.AsNoTracking().Where(b => b.Name.Contains(filter.Name)).ToListAsync();
            var albums = await _dbContext.Albums.AsNoTracking().Where(b => b.Name.Contains(filter.Name)).ToListAsync();
            var songs = await _dbContext.Songs.AsNoTracking().Where(b => b.Name.Contains(filter.Name)).ToListAsync();

            entities.AddRange(_mapper.Map<IEnumerable<SearchResponse>>(bands));
            entities.AddRange(_mapper.Map<IEnumerable<SearchResponse>>(albums));
            entities.AddRange(_mapper.Map<IEnumerable<SearchResponse>>(songs));

            return entities;
        }
    }
}
