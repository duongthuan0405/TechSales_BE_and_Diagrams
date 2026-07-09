using Auth_Module.src.Presentation.Controllers;
using Auth_Module.src.Presentation.Extensions.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth_Module.src.Presentation.Registration
{
    public static partial class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterAuthModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(AuthController).Assembly);
            });

            services.AddServices();
            services.AddRepositories();

            services.AddControllers();
            return services;
        }
    }
}