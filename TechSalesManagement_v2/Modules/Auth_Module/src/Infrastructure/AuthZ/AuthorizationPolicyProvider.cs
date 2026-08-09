using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Auth_Module.src.Infrastructure.AuthZ;

public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }

    override public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if(policyName.StartsWith("Permission:", StringComparison.OrdinalIgnoreCase))
        {
            var permission = policyName.Substring("Permission:".Length);
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new HasPermissionRequirement(permission));
            return Task.FromResult(policy.Build());
        }
        
        return base.GetPolicyAsync(policyName);
    }
}
