using Ecommerce_Website_Backend.Common.Constants;
using Ecommerce_Website_Backend.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Ecommerce_Website_Backend.Extensions
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration config)
        {
            var secretKey = config["JwtSettings:SecretKey"]
                ?? throw new InvalidOperationException("JwtSettings:SecretKey is not configured.");

            var key = Encoding.UTF8.GetBytes(secretKey);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["JwtSettings:Issuer"] ?? "EcommerceApi",
                        ValidAudience = config["JwtSettings:Audience"] ?? "EcommerceClient",
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorizationBuilder()
                .AddPolicy("SuperAdminOnly", p => p.RequireRole(UserRoles.SuperAdmin))
                .AddPolicy("AdminOnly", p => p.RequireRole(UserRoles.Admin))
                .AddPolicy("SuperOrAdmin", p => p.RequireRole(UserRoles.SuperAdmin, UserRoles.Admin))
                .AddPolicy("CustomerOnly", p => p.RequireRole(UserRoles.Customer))
                .AddPolicy("MerchantOnly", p => p.RequireRole(UserRoles.Merchant))
                .AddPolicy("AnyUser", p => p.RequireAuthenticatedUser());

            return services;
        }
    }
}
