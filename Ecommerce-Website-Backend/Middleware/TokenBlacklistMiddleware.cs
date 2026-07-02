using Ecommerce_Website_Backend.Services;
using Ecommerce_Website_Backend.Services.AuthService;

namespace Ecommerce_Website_Backend.Middleware
{
    public class TokenBlacklistMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, LogoutService logoutService)
        {
            var token = context.Request.Headers.Authorization
                .ToString()
                .Replace("Bearer ", string.Empty)
                .Trim();

            if (!string.IsNullOrEmpty(token))
            {
                var isBlacklisted = await logoutService.IsTokenBlacklistedAsync(token);

                if (isBlacklisted)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        status = 401,
                        title = "Unauthorized",
                        detail = "Token has been invalidated. Please login again."
                    });
                    return;
                }
            }

            await next(context);
        }
    }
}
