using Shared.Dtos.Contracts;

namespace Shared.Dtos.Responses
{
    public class BandResponse : IEntityResponse
    {
        public Guid BandId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public long ShortId { get; set; }
        public int FoundingYear { get; set; }
        public bool IsActive { get; set; }
        public bool IsFavorite { get; set; }
        public string ImgUrl { get; set; }
        public IEnumerable<BandAlbumResponse> Albums { get; set; }
        public IEnumerable<string> Genres { get; set; }
    }
}
