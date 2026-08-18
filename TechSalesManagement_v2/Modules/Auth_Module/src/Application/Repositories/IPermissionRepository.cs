namespace Auth_Module.Application.Repositories;

public interface IPermissionRepository
{
    Task<bool> CheckUserHasPermissionAsync(string permission, CancellationToken cancellationToken = default);
}