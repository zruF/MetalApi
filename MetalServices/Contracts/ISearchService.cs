using Shared.Dtos.Requests;
using Shared.Dtos.Responses;

namespace MetalServices.Contracts
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResponse>> GetEntities(SearchFilterRequest filter);
    }
}
