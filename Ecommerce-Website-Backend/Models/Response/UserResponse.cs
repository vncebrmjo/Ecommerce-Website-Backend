using Ecommerce_Website_Backend.Data.Entities;

namespace Ecommerce_Website_Backend.Models.Response
{
    public record UserResponse(
        int Id,
        string UserName,
        string Email,
        string FirstName,
        string LastName,
        string Role,
        bool IsActive)
    {
        public static UserResponse FromEntity(UserEntity user) =>
            new(
                Id: user.Id,
                UserName: user.UserName,
                Email: user.Email,
                FirstName: user.FirstName,
                LastName: user.LastName,
                Role: user.Role,
                IsActive: user.IsActive);
    }
}
