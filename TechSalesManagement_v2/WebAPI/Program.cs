using Test_Module.src.Presentation.Registration;
using WebAPI.Extensions.ServiceProviderExtensions;
using WebAPI.Extensions.ServiceProviderExtensions.Middlewares;

namespace WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Register all modules
        builder.Services.RegisterModules(builder.Configuration);

        // Swagger
        builder.Services.GenerateSwaggerDocument();

        var app = builder.Build();

        app.UseMiddlewares();

        app.MapControllers();
        app.Run();
        

        // Test CI
    }
}
