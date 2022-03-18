namespace MetalModels.Models
{
    public class BandGenre
    {
        public Guid BandId { get; set; }
        public Guid GenreId { get; set; }

        public Band Band { get; set; }
        public Genre Genre { get; set; }
    }
}
