namespace MetalModels.Models
{
    public class BandFavorites
    {
        public Guid UserId { get; set; }
        public Guid BandId { get; set; }

        public User User { get; set; }
        public Band Band { get; set; }
    }
}
