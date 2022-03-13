using MetalModels.Models;
using ScraperService.Contracts;
using Shared.Dtos;

namespace ScraperService
{
    public class ScraperService : IScraperService
    {
        public Task<IEnumerable<Band>> SearchBands(SearchFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
