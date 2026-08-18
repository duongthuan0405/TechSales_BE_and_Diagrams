using WebAPI.Middlewares;

namespace WebAPI.Extension;

public static partial class WebApplicationExtension
{
    internal static WebApplication UseMiddlewares(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
    
            app.UseOpenApi();
            app.UseSwaggerUi();
        }

        app.UseMiddleware<GlobalExceptionCatchingMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
