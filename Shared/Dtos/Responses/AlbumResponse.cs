using MetalModels.Types;
using Shared.Dtos.Contracts;

namespace Shared.Dtos.Responses
{
    public class AlbumResponse : IEntityResponse
    {
        public Guid AlbumId { get; set; }
        public string Name { get; set; }
        public Guid BandId { get; set; }
        public string ReleaseDate { get; set; }
        public AlbumType AlbumType { get; set; }
        public string ImgUrl { get; set; }
        public BandResponse Band { get; set; }
        public IEnumerable<SongResponse> Songs { get; set; }
    }
}
