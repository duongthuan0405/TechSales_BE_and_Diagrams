using System;
using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Domain.Entities;

public class UserToken : BaseEntity
{
    private Guid _userId;
    private string _token = string.Empty;
    private TokenType _type;
    private DateTime _expiredAt;
    private DateTime? _usedAt;

    public Guid UserId
    {
        get => _userId;
        set => _userId = value;
    }

    public string Token
    {
        get => _token;
        set => _token = value ?? string.Empty;
    }

    public TokenType Type
    {
        get => _type;
        set => _type = value;
    }

    public DateTime ExpiredAt
    {
        get => _expiredAt;
        set => _expiredAt = value;
    }

    public DateTime? UsedAt
    {
        get => _usedAt;
        set => _usedAt = value;
    }

    public bool IsExpired => DateTime.UtcNow > _expiredAt;
    public bool IsUsed => _usedAt.HasValue;

    public UserToken(Guid userId, string token, TokenType type, DateTime expiredAt) : base()
    {
        UserId = userId;
        Token = token;
        Type = type;
        ExpiredAt = expiredAt;
    }

    public UserToken() : base() { }
}
