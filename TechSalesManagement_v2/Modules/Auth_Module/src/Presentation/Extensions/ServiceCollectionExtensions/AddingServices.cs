using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth_Module.src.Application.Services;
using Auth_Module.src.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth_Module.src.Presentation.Extensions.ServiceCollectionExtensions
{
    public static partial class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IExecuteAtomically, ExecuteAtomically>();
            return services;
        }
    }
}