using Auth_Module.Application.Repositories;

namespace Auth_Module.Infrastructure.Repositories;

public class PermissionRepository : IPermissionRepository
{
    public Task<bool> CheckUserHasPermissionAsync(string permission, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}