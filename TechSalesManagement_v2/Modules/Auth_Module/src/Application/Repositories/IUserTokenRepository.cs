using Auth_Module.Domain.Entities;
using Auth_Module.Domain.Enums;

namespace Auth_Module.Application.Repositories;

public interface IUserTokenRepository
{
    Task AddAsync(UserToken userToken);
    Task<UserToken> GetByUserIdAndTypeAsync(Guid userId, TokenType type);
    Task UpdateAsync(UserToken existingToken);
}