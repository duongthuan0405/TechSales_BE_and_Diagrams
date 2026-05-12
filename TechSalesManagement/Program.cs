using TechSalesManagement.Presentation_WebAPI.Extensions;

namespace TechSalesManagement;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();

        // Cấu hình Swagger thông qua Extension Method
        builder.Services.AddSwaggerConfiguration();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseSwaggerConfiguration(app.Environment);

        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }
}
