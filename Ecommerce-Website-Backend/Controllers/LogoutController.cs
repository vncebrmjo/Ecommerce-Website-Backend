using Ecommerce_Website_Backend.Services;
using Ecommerce_Website_Backend.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AnyUser")]
    public class LogoutController(LogoutService logoutService) : ControllerBase
    {
        // POST api/logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers.Authorization
                .ToString()
                .Replace("Bearer ", string.Empty)
                .Trim();

            await logoutService.LogoutAsync(token);

            return NoContent(); 
        }
    }
}
