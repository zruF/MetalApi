using MetalModels.Types;

namespace Shared.Dtos.Requests
{
    public class SearchFilterRequest
    {
        public string Name { get; set; }
        public EntityType EntityType { get; set; }
    }
}
