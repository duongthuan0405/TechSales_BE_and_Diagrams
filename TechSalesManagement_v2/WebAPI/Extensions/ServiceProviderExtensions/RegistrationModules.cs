using Auth_Module.src.Presentation.Registration;
using Common_Module.src.Presentation.Registration;
using Test_Module.src.Presentation.Registration;

namespace WebAPI.Extensions.ServiceProviderExtensions
{
    public static partial class ServiceProviderExtension
    {
        public static IServiceCollection RegisterModules(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterCommonModule(configuration);
            services.RegisterTestModule(configuration);
            services.RegisterAuthModule(configuration);
            
            return services;
        }
    }
}