namespace MetalModels.Models
{
    public class GenreFavorites
    {
        public Guid UserId { get; set; }
        public Guid GenreId { get; set; }

        public User User { get; set; }
        public Genre Genre { get; set; }
    }
}
