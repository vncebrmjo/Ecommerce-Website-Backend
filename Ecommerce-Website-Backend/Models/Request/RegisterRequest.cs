using Ecommerce_Website_Backend.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website_Backend.Models.Request
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Email is required")]

        [EmailAddress(ErrorMessage = "Invalid email format")]

        [MaxLength(ValidationConstants.User.EmailMaxLength,
            ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]

        [MinLength(ValidationConstants.User.PasswordMinLength,
            ErrorMessage = "Password must be at least 8 characters")]

        [MaxLength(ValidationConstants.User.PasswordMaxLength,
            ErrorMessage = "Password cannot exceed 100 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]

        [MaxLength(ValidationConstants.User.FirstNameMaxLength,
            ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]

        [MaxLength(ValidationConstants.User.LastNameMaxLength,
            ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]

        [MaxLength(ValidationConstants.User.UserNameMaxLength,
            ErrorMessage = "Username cannot exceed 25 characters")]
        public string UserName { get; set; } = string.Empty;

    }
}
