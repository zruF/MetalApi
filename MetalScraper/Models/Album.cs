namespace MetalScraper.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BandId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Song> Songs { get; set; }
    }
}
