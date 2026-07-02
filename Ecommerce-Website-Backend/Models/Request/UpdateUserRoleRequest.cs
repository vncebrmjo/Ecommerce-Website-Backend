using Ecommerce_Website_Backend.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website_Backend.Models.Request
{
    public class UpdateUserRoleRequest
    {
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = string.Empty;
        public bool IsValidRole() => UserRoles.All.Contains(Role);
    }
}
