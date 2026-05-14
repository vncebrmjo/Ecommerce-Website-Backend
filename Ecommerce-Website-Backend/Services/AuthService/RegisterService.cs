using Ecommerce_Website_Backend.Data;
using Ecommerce_Website_Backend.Data.Entities;
using Ecommerce_Website_Backend.Models.Request;
using Ecommerce_Website_Backend.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Website_Backend.Services.AuthService
{
    public class RegisterService(AppDbContext db)
    {
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var emailExists = await db.Users
                .AnyAsync(u => u.Email == request.Email.ToLower().Trim());

            if (emailExists)
                throw new InvalidOperationException("An account with this email already exists.");

            var userNameExists = await db.Users
                .AnyAsync(u => u.UserName == request.UserName.ToLower().Trim());

            if (userNameExists)
                throw new InvalidOperationException("This username is already taken.");

            var user = new UserEntity
            {
                Email = request.Email.ToLower().Trim(),
                UserName = request.UserName.ToLower().Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return RegisterResponse.FromEntity(user);
        }

    }
}
