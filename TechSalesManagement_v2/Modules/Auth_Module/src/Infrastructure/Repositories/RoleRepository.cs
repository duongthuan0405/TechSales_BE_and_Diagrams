using Auth_Module.Application.Repositories;
using Auth_Module.Domain.Entities;

namespace Auth_Module.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    public Task<Role> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}