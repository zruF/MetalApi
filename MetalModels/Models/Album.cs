using MetalModels.Contracts;
using MetalModels.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MetalModels.Models
{
    public class Album : IEntity
    {
        [Key]
        public Guid AlbumId { get; set; }
        public string Name { get; set; }
        [ForeignKey("Band")]
        public Guid BandId { get; set; }
        public int ReleaseYear { get; set; }
        public AlbumType AlbumType { get; set; }
        public IEnumerable<Song> Songs { get; set; }
    }
}
