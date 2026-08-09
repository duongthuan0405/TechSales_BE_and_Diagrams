using Auth_Module.src.Infrastructure.AuthZ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Auth_Module.src.Presentation.Extensions.ServiceCollectionExtensions
{
    public static partial class ServiceCollectionExtension
    {
        public static IServiceCollection AddAuthZ(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, HasPermissionHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddAuthorization();
            return services;
        }
    }
}