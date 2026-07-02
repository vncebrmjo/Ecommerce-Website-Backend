using Ecommerce_Website_Backend.Configuration;
using Ecommerce_Website_Backend.Data;
using Ecommerce_Website_Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ecommerce_Website_Backend.Services.AuthService
{
    public class LogoutService(AppDbContext db, IOptions<JwtSettings> jwtOptions)
    {
        private readonly JwtSettings _jwt = jwtOptions.Value;
        public async Task LogoutAsync(string token)
        {
            // Check if already blacklisted
            var exists = await db.TokenBlacklist
                .AnyAsync(t => t.SessionToken == token);

            if (exists) return;

            db.TokenBlacklist.Add(new TokenBlacklistEntity
            {
                SessionToken = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes) 
            });

            await db.SaveChangesAsync();
        }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            return await db.TokenBlacklist
                .AnyAsync(t => t.SessionToken == token
                    && t.ExpiresAt > DateTime.UtcNow);
        }

        public async Task CleanupExpiredTokensAsync()
        {
            var expired = await db.TokenBlacklist
                .Where(t => t.ExpiresAt <= DateTime.UtcNow)
                .ToListAsync();

            db.TokenBlacklist.RemoveRange(expired);
            await db.SaveChangesAsync();
        }



    }
}
