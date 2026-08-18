using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common_Module.Extensions;
public static partial class ServiceCollectionExtension
{
    public static IServiceCollection RegisterCommonModule(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}