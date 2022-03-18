using MetalModels.Types;

namespace Shared.Dtos.Responses
{
    public class AlbumResponse
    {
        public Guid AlbumId { get; set; }
        public string Name { get; set; }
        public Guid BandId { get; set; }
        public int ReleaseYear { get; set; }
        public AlbumType AlbumType { get; set; }
        public IEnumerable<SongResponse> Songs { get; set; }
    }
}
