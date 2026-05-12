using System;

namespace TechSalesManagement.Domain.Common;

public abstract class BaseEntity
{
    private Guid _id;
    private DateTime _createdAt;
    private DateTime? _updatedAt;

    public Guid Id
    {
        get => _id;
        set => _id = value;
    }

    public DateTime CreatedAt
    {
        get => _createdAt;
        set => _createdAt = value;
    }

    public DateTime? UpdatedAt
    {
        get => _updatedAt;
        set => _updatedAt = value;
    }

    protected BaseEntity()
    {
        _id = Guid.NewGuid();
        _createdAt = DateTime.UtcNow;
    }
}
