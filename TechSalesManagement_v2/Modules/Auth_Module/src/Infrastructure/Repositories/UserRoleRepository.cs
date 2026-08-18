using Auth_Module.Application.Repositories;
using Auth_Module.Domain.Entities;

namespace Auth_Module.Infrastructure.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    public Task AddAsync(UserRole userRole, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}