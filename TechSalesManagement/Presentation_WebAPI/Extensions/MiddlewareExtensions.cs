using TechSalesManagement.Presentation_WebAPI.Middlewares;

namespace TechSalesManagement.Presentation_WebAPI.Extensions;

public static class MiddlewareExtensions
{
    public static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<GlobalExceptionMiddleware>();
        services.AddTransient<RequestLoggingMiddleware>();
        return services;
    }

    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        // 1. Exception Handling should always be first to catch all subsequent errors
        app.UseMiddleware<GlobalExceptionMiddleware>();

        // 2. Request Logging to monitor traffic and performance
        app.UseMiddleware<RequestLoggingMiddleware>();

        // 3. Swagger (Dev environment only)
        app.UseSwaggerConfiguration(env);

        // 4. Security & Auth
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
