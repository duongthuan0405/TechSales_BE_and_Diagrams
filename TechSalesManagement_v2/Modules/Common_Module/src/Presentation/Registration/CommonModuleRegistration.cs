using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common_Module.src.Middlewares;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common_Module.src.Presentation.Registration
{
    public static partial class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterCommonModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<GlobalExceptionCatchingMiddleware>();
            return services;
        }
    }
}