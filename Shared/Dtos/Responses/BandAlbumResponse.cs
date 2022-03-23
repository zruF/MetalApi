using MetalModels.Types;

namespace Shared.Dtos.Responses
{
    public class BandAlbumResponse
    {
        public Guid AlbumId { get; set; }
        public string Name { get; set; }
        public int ReleaseYear { get; set; }
        public AlbumType AlbumType { get; set; }
        public string ImgUrl { get; set; }
    }
}
