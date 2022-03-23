using MetalServices.Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Responses;

namespace MetalAPI.Controllers
{
    [Route("[controller]")]
    public class BandController : BaseController
    {
        private readonly IBandService _bandService;

        public BandController(IServiceProvider serviceProvider)
        {
            _bandService = serviceProvider.GetService<IBandService>();
        }

        [HttpGet("{bandId}")]
        [ProducesResponseType(typeof(BandResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBand(Guid bandId)
        {
            var band = await _bandService.GetBandAsync(bandId);
            return Ok(band);
        }

        [HttpPut("Favorite")]
        [ProducesResponseType(typeof(BandResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetFavorite(Guid bandId)
        {
            var band = await _bandService.SetFavoriteAsync(bandId);
            return Ok(band);
        }

        [HttpPut("Unfavorite")]
        [ProducesResponseType(typeof(BandResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetUnfavorite(Guid bandId)
        {
            var band = await _bandService.SetUnfavoriteAsync(bandId);
            return Ok(band);
        }
    }
}
