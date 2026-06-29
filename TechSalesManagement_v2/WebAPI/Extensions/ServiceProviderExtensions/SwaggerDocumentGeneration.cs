using NSwag;

namespace WebAPI.Extensions.ServiceProviderExtensions
{
    public static partial class ServiceProviderExtension
    {
        public static IServiceCollection GenerateSwaggerDocument(this IServiceCollection services)
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
}