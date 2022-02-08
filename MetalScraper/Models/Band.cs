namespace MetalScraper.Models
{
    public class Band
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Genre> Genres { get; set; }
    }
}
