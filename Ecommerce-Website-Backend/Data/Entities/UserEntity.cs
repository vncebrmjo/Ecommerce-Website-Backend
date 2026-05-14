using Ecommerce_Website_Backend.Common.Constants;

namespace Ecommerce_Website_Backend.Data.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = UserRoles.Customer;
        public bool IsActive { get; set; } = true;

    }
}
