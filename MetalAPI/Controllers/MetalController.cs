using Microsoft.AspNetCore.Mvc;

namespace MetalAPI.Controllers
{
    public class MetalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
