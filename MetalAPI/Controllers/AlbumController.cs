using MetalServices.Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Responses;
using Shared.Dtos.Responses.Pagination;

namespace MetalAPI.Controllers
{
    [Route("[controller]")]
    public class AlbumController : BaseController
    {
        private readonly IAlbumService _albumService;
        public AlbumController(IServiceProvider serviceProvider)
        {
            _albumService = serviceProvider.GetService<IAlbumService>();
        }

        [HttpGet("{albumId}")]
        [ProducesResponseType(typeof(AlbumResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAlbum(Guid albumId)
        {
            var album = await _albumService.GetAlbumAsync(albumId);
            return Ok(album);
        }

        [HttpGet("Random")]
        [ProducesResponseType(typeof(AlbumResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRandom()
        {
            var album = await _albumService.GetRandomAsync();
            return Ok(album);
        }

        [HttpGet("Upcoming")]
        [ProducesResponseType(typeof(PaginationResponse<AlbumResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUpcomingAlbums([FromQuery] bool favorites)
        {
            var albums = await _albumService.GetUpcomingAlbums(favorites);
            return Ok(albums);
        }
    }
}
