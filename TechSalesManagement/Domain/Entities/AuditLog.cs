using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class AuditLog : BaseEntity
{
    private Guid? _userId;
    private string _action = string.Empty;
    private string _tableName = string.Empty;
    private string _primaryKey = string.Empty;
    private string? _oldValues;
    private string? _newValues;
    private string? _affectedColumns;

    public Guid? UserId
    {
        get => _userId;
        set => _userId = value;
    }

    public string Action
    {
        get => _action;
        set => _action = value ?? string.Empty;
    }

    public string TableName
    {
        get => _tableName;
        set => _tableName = value ?? string.Empty;
    }

    public string PrimaryKey
    {
        get => _primaryKey;
        set => _primaryKey = value ?? string.Empty;
    }

    public string? OldValues
    {
        get => _oldValues;
        set => _oldValues = value;
    }

    public string? NewValues
    {
        get => _newValues;
        set => _newValues = value;
    }

    public string? AffectedColumns
    {
        get => _affectedColumns;
        set => _affectedColumns = value;
    }

    public AuditLog(string action, string tableName, string primaryKey) : base()
    {
        Action = action;
        TableName = tableName;
        PrimaryKey = primaryKey;
    }

    public AuditLog() : base() { }
}
