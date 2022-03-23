using MetalServices.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MetalAPI.Controllers
{
    [Route("[controller]")]
    public class GenreController : BaseController
    {
        private readonly IGenreService _genreService;

        public GenreController(IServiceProvider serviceProvider)
        {
            _genreService = serviceProvider.GetService<IGenreService>();
        }

        [HttpGet("common")]
        public async Task<IActionResult> GetCommonGenres()
        {
            var commonGenres = await _genreService.GetCommonGenres();

            return Ok(commonGenres);
        }
    }
}
