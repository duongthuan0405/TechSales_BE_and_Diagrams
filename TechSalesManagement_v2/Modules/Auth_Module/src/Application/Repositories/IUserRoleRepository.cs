using Auth_Module.Domain.Entities;

namespace Auth_Module.Application.Repositories;

public interface IUserRoleRepository
{
    Task AddAsync(UserRole userRole, CancellationToken cancellationToken = default);
}