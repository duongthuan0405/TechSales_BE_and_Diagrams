using System;
using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Domain.Entities;

public class UserToken : BaseEntity
{
    public Guid user_id { get; set; }
    public string token { get; set; } = string.Empty;
    public TokenType type { get; set; }
    public DateTimeOffset expired_at { get; set; }
    public DateTimeOffset? used_at { get; set; }

    public UserToken(Guid userId, string token, TokenType type, DateTimeOffset expiredAt)
    {
        user_id = userId;
        this.token = token;
        this.type = type;
        expired_at = expiredAt;
    }

    public UserToken() { }
}
