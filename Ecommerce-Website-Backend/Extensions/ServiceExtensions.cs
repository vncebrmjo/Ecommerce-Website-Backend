using Ecommerce_Website_Backend.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website_Backend.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddValidationErrorHandling(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(kvp => kvp.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    var error = new ApiErrorResponse
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation Failed",
                        Errors = errors
                    };

                    return new BadRequestObjectResult(error);
                };
            });

            return services;
        }
    }
}
