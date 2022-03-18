using MetalServices.Contracts;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAlbum(Guid albumId)
        {
            var album = await _albumService.GetAlbumAsync(albumId);
            return (IActionResult)Ok(album);
        }
    }
}
