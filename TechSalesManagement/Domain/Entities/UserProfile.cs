using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class UserProfile : BaseEntity
{
    public Guid user_id { get; set; }
    public string full_name { get; set; } = string.Empty;
    public string phone { get; set; } = string.Empty;
    public string? avatar_url { get; set; }
    public DateTimeOffset? date_of_birth { get; set; }

    public UserProfile(Guid userId, string fullName, string phone)
    {
        user_id = userId;
        full_name = fullName;
        this.phone = phone;
    }

    public UserProfile() { }
}
