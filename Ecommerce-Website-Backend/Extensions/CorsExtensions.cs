using Ecommerce_Website_Backend.Configuration;

namespace Ecommerce_Website_Backend.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            var corsOptions = configuration
                .GetSection(CorsOptions.SectionName)
                .Get<CorsOptions>()
                ?? throw new InvalidOperationException("Cors configuration is missing.");

            services.AddCors(options =>
            {
                options.AddPolicy(corsOptions.PolicyName, policy =>
                {
                    policy.WithOrigins(corsOptions.AllowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowedToAllowWildcardSubdomains();
                });
            });

            services.AddSingleton(corsOptions);

            return services;
        }
    }
}
