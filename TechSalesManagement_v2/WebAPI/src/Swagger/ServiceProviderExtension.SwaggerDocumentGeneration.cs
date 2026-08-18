using NSwag;
public static partial class ServiceProviderExtension
{
    internal static IServiceCollection GenerateSwaggerDocument(this IServiceCollection services)
    {
        services.AddOpenApiDocument(options => {
            options.PostProcess = document =>
            {
                document.Info = new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TechSales API",
                    Description = "An ASP.NET Core Web API for TechSales",
                };
            };
        });

        return services;
    }
}
