using Ecommerce_Website_Backend.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website_Backend.Models.Request
{
    public class LoginRequest
    {

        [Required(ErrorMessage = "UserName is required")]

        [MaxLength(ValidationConstants.User.UserNameMaxLength,
            ErrorMessage = "UserName cannot exceed 25 characters")]

        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]

        [MinLength(ValidationConstants.User.PasswordMinLength,
            ErrorMessage = "Password must be at least 8 characters")]

        [MaxLength(ValidationConstants.User.PasswordMaxLength,
            ErrorMessage = "Password cannot exceed 100 characters")]
        public string Password { get; set; } = string.Empty;

    }
}
