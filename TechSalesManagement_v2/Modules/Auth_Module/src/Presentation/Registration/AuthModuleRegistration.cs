using Auth_Module.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth_Module.Presentation.Registration
{
    public static partial class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterAuthModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices();
            services.AddRepositories();
            services.AddControllers();
            services.AddAuthN(configuration);
            services.AddAuthZ();
            
            return services;
        }
    }
}