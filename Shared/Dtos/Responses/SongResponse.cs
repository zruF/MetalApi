namespace Shared.Dtos.Responses
{
    public class SongResponse
    {
        public Guid SongId { get; set; }
        public Guid AlbumId { get; set; }
        public string Name { get; set; }
        public string Length { get; set; }
    }
}
