using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth_Module.src.Presentation.Registration
{
    public static partial class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterAuthModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            return services;
        }
    }
}