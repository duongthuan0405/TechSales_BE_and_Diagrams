using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth_Module.Domain.Entities;

namespace Auth_Module.Application.Repositories
{
    public interface IUserRepository
    {
        Task<Guid> AddAsync(User newUser, CancellationToken cancellationToken = default);
        Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task UpdateAsync(User existingUser, CancellationToken cancellationToken = default);
    }
}