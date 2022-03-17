using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MetalModels.Models
{
    public class Genre
    {
        [Key]
        public Guid GenreId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Band> Bands { get; set; }
        [ForeignKey("GenreId")]
        public virtual ICollection<BandGenres> BandGenres { get; set; }
    }
}
