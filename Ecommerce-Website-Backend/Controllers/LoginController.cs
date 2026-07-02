using Ecommerce_Website_Backend.Models.Request;
using Ecommerce_Website_Backend.Services;
using Ecommerce_Website_Backend.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController(LoginService loginService) : ControllerBase
    {
        // POST api/login
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await loginService.LoginAsync(request);
            return Ok(response);
        }
    }
}
