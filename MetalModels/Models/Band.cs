using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MetalModels.Models
{
    public class Band
    {
        [Key]
        public Guid BandId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public long ShortId { get; set; }
        public int FoundingYear { get; set; }
        public bool IsActive { get; set; }
        public bool IsFavorite { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Album> Albums { get; set; }
        [ForeignKey("BandId")]
        public virtual ICollection<BandGenres> BandGenres { get; set; }
    }
}
