using Ecommerce_Website_Backend.Models.Request;
using Ecommerce_Website_Backend.Services;
using Ecommerce_Website_Backend.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController(RegisterService registerService) : ControllerBase
    {
        // POST api/register
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await registerService.RegisterAsync(request);
            return CreatedAtAction(nameof(Register), response);
        }
    }
}
