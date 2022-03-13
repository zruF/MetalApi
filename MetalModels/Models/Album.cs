using MetalModels.Types;

namespace MetalModels.Models
{
    public class Album
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long BandId { get; set; }
        public int ReleaseYear { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Song> Songs { get; set; }
        public AlbumType AlbumType { get; set; }
    }
}
