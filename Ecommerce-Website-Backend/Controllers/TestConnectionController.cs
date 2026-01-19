using Ecommerce_Website_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Ecommerce_Website_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class TestConnectionController : ControllerBase
    {


        [HttpGet]
        [ProducesResponseType(typeof(CheckResponse), StatusCodes.Status200OK)]
        public ActionResult<CheckResponse> GetHealth()
        {
          

            var response = new CheckResponse
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
               
                
            };

            return Ok(response);
        }
    }



}



