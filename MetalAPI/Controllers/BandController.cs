using MetalServices.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MetalAPI.Controllers
{
    public class BandController : Controller
    {
        private readonly IBandService _bandService;

        public BandController(IServiceProvider serviceProvider)
        {
            _bandService = serviceProvider.GetService<IBandService>();
        }

        [HttpGet("{bandId}")]
        [ExceptionHandling]
        public async Task<IActionResult> GetBand(Guid bandId)
        {
            var band = await _bandService.GetBandAsync(bandId);
            return Ok(band);
        }
    }
}
