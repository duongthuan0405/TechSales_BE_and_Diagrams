using Microsoft.AspNetCore.Authorization;

namespace Auth_Module.Infrastructure.AuthZ;

public class HasPermissionRequirement : IAuthorizationRequirement
{
    private readonly string _permission = "";
    
    public HasPermissionRequirement(string permission)
    {
        _permission = permission;
    }

    public string Permission => _permission;
}
