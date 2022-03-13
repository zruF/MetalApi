namespace MetalModels.Models
{
    public class Band
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public int FoundingYear { get; set; }
        public IEnumerable<Album> Albums { get; set; }
        public bool IsActive { get; set; }
    }
}
