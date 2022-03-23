using MetalModels.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MetalModels.Models
{
    public class Band : IEntity
    {
        [Key]
        public Guid BandId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string ShortId { get; set; }
        public int FoundingYear { get; set; }
        public bool IsActive { get; set; }
        public string ImgUrl { get; set; }
        public IEnumerable<Album> Albums { get; set; }
        public virtual ICollection<BandFavorites> Favorites { get; set; }
        public virtual ICollection<BandGenre> BandGenres { get; set; }
    }
}
