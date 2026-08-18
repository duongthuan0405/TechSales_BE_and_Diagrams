using Auth_Module.Domain.Entities;

namespace Auth_Module.Application.Repositories;

public interface IRoleRepository
{
    Task<Role> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}