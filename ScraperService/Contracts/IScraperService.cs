using MetalModels.Models;
using Shared.Dtos;

namespace ScraperService.Contracts
{
    public interface IScraperService
    {
        public Task<IEnumerable<Band>> SearchBands(SearchFilter filter);
    }
}
