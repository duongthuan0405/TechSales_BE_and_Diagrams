using Auth_Module.Application.Repositories;
using Auth_Module.Domain.Entities;

namespace Auth_Module.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public Task<Guid> AddAsync(User newUser, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(User existingUser, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}