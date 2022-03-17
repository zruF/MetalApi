using System.ComponentModel.DataAnnotations.Schema;

namespace MetalModels.Models
{
    public class BandGenres
    {
        public Guid BandId { get; set; }
        public Guid GenreId { get; set; }

        [ForeignKey("Band")]
        public Band Band { get; set; }
        [ForeignKey("Genre")]
        public Genre Genre { get; set; }
    }
}
