using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid user_id { get; set; }
    public string title { get; set; } = string.Empty;
    public string content { get; set; } = string.Empty;
    public bool is_read { get; set; }
    public Guid? ref_to { get; set; }

    public Notification(Guid userId, string title, string content)
    {
        user_id = userId;
        this.title = title;
        this.content = content;
        is_read = false;
    }

    public Notification() { }
}
