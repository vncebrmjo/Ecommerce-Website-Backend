using Ecommerce_Website_Backend.Common.Constants;
using Ecommerce_Website_Backend.Data;
using Ecommerce_Website_Backend.Data.Entities;

namespace Ecommerce_Website_Backend.Extensions
{
    public static class SuperAdminCreationExtensions
    {
        public static async Task CreateSuperAdminAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var email = config["SuperAdmin:Email"]
                ?? throw new InvalidOperationException("SuperAdmin:Email is not configured.");
            var userName = config["SuperAdmin:UserName"]
                ?? throw new InvalidOperationException("SuperAdmin:UserName is not configured.");
            var password = config["SuperAdmin:Password"]
                ?? throw new InvalidOperationException("SuperAdmin:Password is not configured.");

            if (!db.Users.Any(u => u.Role == UserRoles.SuperAdmin))
            {
                db.Users.Add(new UserEntity
                {
                    FirstName = "SuperAdmin FirstName",
                    LastName = "SuperAdmin LastName",
                    Email = email,
                    UserName = userName,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                    Role = UserRoles.SuperAdmin,
                    IsActive = true
                });

                await db.SaveChangesAsync();
            }
        }
    }
}
