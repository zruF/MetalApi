using MetalServices.Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Requests;
using Shared.Dtos.Responses;

namespace MetalAPI.Controllers
{
    [Route("[controller]")]
    public class SearchController : BaseController
    {
        private readonly ISearchService _searchService;

        public SearchController(IServiceProvider serviceProvider)
        {
            _searchService = serviceProvider.GetService<ISearchService>();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SearchResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBand(SearchFilterRequest request)
        {
            var entities = await _searchService.GetEntities(request);
            return Ok(entities);
        }
    }
}
