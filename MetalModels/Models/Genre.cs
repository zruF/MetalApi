using MetalModels.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MetalModels.Models
{
    public class Genre : IEntity
    {
        [Key]
        public Guid GenreId { get; set; }
        public string Name { get; set; }
        public ICollection<GenreFavorites> Favorites { get; set; }
        public ICollection<BandGenre> BandGenres { get; set; }
    }
}
