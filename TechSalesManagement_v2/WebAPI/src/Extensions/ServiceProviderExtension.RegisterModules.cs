using Auth_Module.Presentation.Registration;
using Common_Module.Extensions;
using Test_Module.Extensions;

namespace WebAPI.Extension;

public static partial class ServiceProviderExtension
{
    internal static IServiceCollection RegisterModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterCommonModule(configuration);
        services.RegisterTestModule(configuration);
        services.RegisterAuthModule(configuration);
        
        return services;
    }
}
