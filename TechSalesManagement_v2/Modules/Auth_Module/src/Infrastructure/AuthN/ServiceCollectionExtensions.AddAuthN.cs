using Auth_Module.Infrastructure.AuthN;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth_Module.Extensions;

internal static partial class ServiceCollectionExtensions
{
    internal static IServiceCollection AddAuthN(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigAuthJwtBearer(configuration);

        return services;
    }
}
