using MetalServices.Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Requests;

namespace MetalAPI.Controllers
{
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IServiceProvider serviceProvider)
        {
            _userService = serviceProvider.GetService<IUserService>();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewUser(UserRequest request)
        {
            var user = await _userService.CreateNewUser(request);
            return Ok(user);
        }
    }
}
