using Ecommerce_Website_Backend.Data.Entities;

namespace Ecommerce_Website_Backend.Models.Response
{
    public record RegisterResponse(
        int Id,
        string FirstName,
        string LastName,
        string Email,
        string UserName,
        string Role)
    {
        public static RegisterResponse FromEntity(UserEntity user) =>
            new(
                Id: user.Id,
                FirstName: user.FirstName,
                LastName: user.LastName,
                Email: user.Email,
                UserName: user.UserName,
                Role: user.Role);
    }
}
