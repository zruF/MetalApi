using Microsoft.AspNetCore.Mvc;

namespace MetalAPI.Controllers
{
    public class BandController : Controller
    {
        public BandController(IServiceProvider serviceProvider)
        {
            [HttpGet("{bandId}")]
            public IActionResult GetBand(Guid bandId)
            {
                return Ok();
            }
        }
    }
}
