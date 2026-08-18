using MediatR;
using WebAPI.Extension;
using WebAPI.Middlewares;

namespace WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string environmentVariablesPath = builder.Configuration["PATHS:EnvironmentVariables"] ?? "./src/EnvironmentVariables_fake";

        if(builder.Environment.IsDevelopment())
        {
            builder.Configuration.LoadEnvironmentVariables(environmentVariablesPath);  
        }

        // Register all modules
        builder.Services.RegisterModules(builder.Configuration);
       
        // Swagger
        builder.Services.GenerateSwaggerDocument();

        builder.Services.AddScoped<GlobalExceptionCatchingMiddleware>();

        var app = builder.Build();


        app.UseMiddlewares();

        app.MapControllers();
        app.Run();
    }
}
