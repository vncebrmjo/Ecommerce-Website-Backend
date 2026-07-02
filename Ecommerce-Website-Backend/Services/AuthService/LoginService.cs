using Ecommerce_Website_Backend.Data;
using Ecommerce_Website_Backend.Helpers;
using Ecommerce_Website_Backend.Models.Request;
using Ecommerce_Website_Backend.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Website_Backend.Services.AuthService
{
    public class LoginService(AppDbContext db, JwtHelper jwtHelper)
    {
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await db.Users
                .FirstOrDefaultAsync(u => u.UserName == request.UserName.ToLower().Trim());

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password.");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("This account has been deactivated.");

            var (token, expiresAt) = jwtHelper.GenerateToken(user);

            return new LoginResponse(
                Token: token,
                FirstName: user.FirstName,
                LastName: user.LastName,
                Email: user.Email,
                UserName: user.UserName,
                Role: user.Role,
                ExpiresAt: expiresAt);
        }
    }
}
