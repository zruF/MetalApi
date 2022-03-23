using Shared.Dtos.Contracts;

namespace Shared.Dtos.Responses
{
    public class SongResponse : IEntityResponse
    {
        public Guid SongId { get; set; }
        public Guid AlbumId { get; set; }
        public string Name { get; set; }
        public string Length { get; set; }
    }
}
