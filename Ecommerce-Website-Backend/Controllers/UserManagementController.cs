using Ecommerce_Website_Backend.Common.Constants;
using Ecommerce_Website_Backend.Models.Request;
using Ecommerce_Website_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce_Website_Backend.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Policy = "SuperOrAdmin")]  
    public class UserManagementController(UserManagementService userManagementService)
        : ControllerBase
    {
        // Reads the requesting user's role from their JWT
        private string RequestingUserRole =>
            User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

        // GET api/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await userManagementService.GetAllAsync();
            return Ok(users);
        }

        // GET api/users/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await userManagementService.GetByIdAsync(id);
            return Ok(user);
        }

        // PATCH api/users/1/role
        [HttpPatch("{id:int}/role")]
        public async Task<IActionResult> UpdateRole(
            int id,
            [FromBody] UpdateUserRoleRequest request)
        {
            var user = await userManagementService
                .UpdateRoleAsync(id, request, RequestingUserRole);
            return Ok(user);
        }

        // PATCH api/users/1/status
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var user = await userManagementService
                .ToggleStatusAsync(id, RequestingUserRole);
            return Ok(user);
        }

        // DELETE api/users/1
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "SuperAdminOnly")]  // override — delete is SuperAdmin only
        public async Task<IActionResult> Delete(int id)
        {
            await userManagementService.DeleteAsync(id, RequestingUserRole);
            return NoContent();
        }
    }
}
