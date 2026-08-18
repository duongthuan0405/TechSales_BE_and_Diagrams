using Auth_Module.Domain.Enums;

namespace Auth_Module.Domain.Entities;

public class UserToken
{
    private DateTimeOffset _createdAt = DateTimeOffset.UtcNow;
    private Guid _id = Guid.Empty;
    private DateTimeOffset? _updatedAt =  null; 
    private Guid _userId = Guid.Empty;
    private string _token = string.Empty;
    private TokenType _type;
    private DateTimeOffset _expiredAt = DateTimeOffset.UtcNow;
    private DateTimeOffset? _usedAt = null;

    public DateTimeOffset CreatedAt { get => _createdAt; set => _createdAt = value; }
    public Guid Id { get => _id; set => _id = value; }
    public DateTimeOffset? UpdatedAt { get => _updatedAt; set => _updatedAt = value; }
    public Guid UserId { get => _userId; set => _userId = value; }
    public string Token { get => _token; set => _token = value; }
    public TokenType Type { get => _type; set => _type = value; }
    public DateTimeOffset ExpiredAt { get => _expiredAt; set => _expiredAt = value; }
    public DateTimeOffset? UsedAt { get => _usedAt; set => _usedAt = value; }

    public UserToken(Guid userId, string token, TokenType type, DateTimeOffset expiredAt)
    {
        this.UserId = userId;
        this.Token = token;
        this.Type = type;
        this.ExpiredAt = expiredAt;
    }

    public UserToken() { }
}
