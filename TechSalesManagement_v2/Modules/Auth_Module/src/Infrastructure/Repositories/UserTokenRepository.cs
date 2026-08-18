using Auth_Module.Application.Repositories;
using Auth_Module.Domain.Entities;
using Auth_Module.Domain.Enums;

namespace Auth_Module.Infrastructure.Repositories;

public class UserTokenRepository : IUserTokenRepository
{
    public Task AddAsync(UserToken userToken)
    {
        throw new NotImplementedException();
    }

    public Task<UserToken> GetByUserIdAndTypeAsync(Guid userId, TokenType type)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(UserToken existingToken)
    {
        throw new NotImplementedException();
    }
}