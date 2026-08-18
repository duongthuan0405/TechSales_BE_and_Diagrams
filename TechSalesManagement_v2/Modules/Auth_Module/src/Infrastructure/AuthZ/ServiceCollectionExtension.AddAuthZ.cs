using Auth_Module.Infrastructure.AuthZ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Auth_Module.Extensions
{
    internal static partial class ServiceCollectionExtension
    {
        internal static IServiceCollection AddAuthZ(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, HasPermissionHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddAuthorization();
            return services;
        }
    }
}