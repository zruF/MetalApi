namespace MetalModels.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string MacAddress { get; set; }
        public string Smartphone { get; set; }
        public string AndroidVersion { get; set; }
        public virtual ICollection<BandFavorites> BandFavorites { get; set;}
        public virtual ICollection<GenreFavorites> GenreFavorites { get; set;}
    }
}
