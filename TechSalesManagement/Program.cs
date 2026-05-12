using TechSalesManagement.Presentation_WebAPI.Extensions;
using TechSalesManagement.Infrastructure.Persistence;

namespace TechSalesManagement;

public class Program
{
    public static void Main(string[] args)
    {
        // Load environment variables from .env.development
        DotNetEnv.Env.Load(".env.development");

        var builder = WebApplication.CreateBuilder(args);

        // Enable Serilog Logging
        builder.Host.UseSerilogConfiguration();

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();

        // Configure Swagger via Extension Method
        builder.Services.AddSwaggerConfiguration();
        
        // Configure Middleware services
        builder.Services.AddMiddlewares();
        // Configure Validation
        builder.Services.AddValidationConfiguration();
        // Register Persistence (Database)
        builder.Services.AddPersistence();

        var app = builder.Build();

        // Configure Middleware Pipeline
        app.UseMiddlewares(app.Environment);

        app.MapControllers();
        app.Run();
    }
}
