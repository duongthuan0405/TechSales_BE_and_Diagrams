using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Middlewares;

namespace Common_Module.Extensions;

public static partial class ServiceCollectionExtension {
    internal static IServiceCollection AddMiddlewares(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<GlobalExceptionCatchingMiddleware>();
        return services;
    }
}