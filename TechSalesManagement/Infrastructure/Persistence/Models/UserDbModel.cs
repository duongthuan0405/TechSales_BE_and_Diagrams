using System;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Infrastructure.Persistence.Models;

public class UserDbModel
{
    public Guid id { get; set; }
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public UserStatus status { get; set; } = UserStatus.PENDING;
    public int failed_login_attempts { get; set; }
    public DateTimeOffset created_at { get; set; }
    public DateTimeOffset? updated_at { get; set; }
    public DateTimeOffset? last_failed_at { get; set; }
    public DateTimeOffset? locked_until { get; set; }
}
