using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class AuditLog : BaseEntity
{
    public Guid? user_id { get; set; }
    public string action { get; set; } = string.Empty;
    public string table_name { get; set; } = string.Empty;
    public string primary_key { get; set; } = string.Empty;
    public string? old_values { get; set; }
    public string? new_values { get; set; }
    public string? affected_columns { get; set; }

    public AuditLog(Guid? userId, string action, string tableName, string primaryKey)
    {
        user_id = userId;
        this.action = action;
        table_name = tableName;
        primary_key = primaryKey;
    }

    public AuditLog() { }
}
