using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth_Module.Application.Services;
using Auth_Module.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth_Module.Extensions
{
    internal static partial class ServiceCollectionExtension
    {
        internal static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IExecuteAtomically, ExecuteAtomically>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IOtpService, OtpService>();
            return services;
        }
    }
}