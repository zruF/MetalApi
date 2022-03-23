using Shared.Dtos.Contracts;

namespace Shared.Dtos.Responses
{
    public class SearchResponse : IEntityResponse
    {
        public string Name { get; set; }
        public string EntityType { get; set; }
    }
}
