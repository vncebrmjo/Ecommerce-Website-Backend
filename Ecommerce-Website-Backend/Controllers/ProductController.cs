using Ecommerce_Website_Backend.Common.Constants;
using Ecommerce_Website_Backend.Models.Request;
using Ecommerce_Website_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ecommerce_Website_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(ProductService service) : ControllerBase
    {
        // The user id can come through as either ClaimTypes.NameIdentifier or the
        //raw "sub" claim, depending on JWT config — check both to be safe.
        private int CallerId =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? throw new UnauthorizedAccessException("Token is missing a user id claim."));

        private string CallerRole =>
            User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

        // GET api/product
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var products = await service.GetAllAsync(ct);
            return Ok(products);
        }

        // GET api/product/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var product = await service.GetByIdAsync(id, ct);
            return Ok(product);
        }

        // POST api/product
        [HttpPost]
        [Authorize(Roles = UserRoles.ProductManagement)]
        public async Task<IActionResult> Create([FromBody] ProductRequest request, CancellationToken ct)
        {
            var product = await service.CreateAsync(request, CallerId, CallerRole, ct);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // PUT api/product/1
        [HttpPut("{id:int}")]
        [Authorize(Roles = UserRoles.ProductManagement)]
        public async Task<IActionResult> Update(int id, [FromBody] ProductRequest request, CancellationToken ct)
        {
            var product = await service.UpdateAsync(id, request, CallerId, CallerRole, ct);
            return Ok(product);
        }

        // DELETE api/product/1
        [HttpDelete("{id:int}")]
        [Authorize(Roles = UserRoles.ProductManagement)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await service.DeleteAsync(id, CallerId, CallerRole, ct);
            return NoContent();
        }
    }
}