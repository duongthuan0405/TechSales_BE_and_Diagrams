using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class Notification : BaseEntity
{
    private Guid _userId;
    private string _title = string.Empty;
    private string _content = string.Empty;
    private bool _isRead;
    private Guid? _refTo;

    public Guid UserId
    {
        get => _userId;
        set => _userId = value;
    }

    public string Title
    {
        get => _title;
        set => _title = value ?? string.Empty;
    }

    public string Content
    {
        get => _content;
        set => _content = value ?? string.Empty;
    }

    public bool IsRead
    {
        get => _isRead;
        set => _isRead = value;
    }

    public Guid? RefTo
    {
        get => _refTo;
        set => _refTo = value;
    }

    public Notification(Guid userId, string title, string content) : base()
    {
        UserId = userId;
        Title = title;
        Content = content;
        IsRead = false;
    }

    public Notification() : base() { }
}
