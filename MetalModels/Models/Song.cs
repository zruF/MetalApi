using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MetalModels.Models
{
    public class Song
    {
        [Key]
        public Guid SongId { get; set; }
        [ForeignKey("Album")]
        public Guid AlbumId { get; set; }
        public string Name { get; set; }
        public string Length { get; set; }
    }
}
