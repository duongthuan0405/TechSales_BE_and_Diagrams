using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Test_Module.Extensions
{
    public static partial class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterTestModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            return services;
        }
    }
}