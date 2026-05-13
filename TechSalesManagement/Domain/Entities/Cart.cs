using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class Cart
{
    public Guid id { get; set; }
    public DateTimeOffset createdAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? updatedAt { get; set; }

    public Guid userId { get; set; }
    public List<CartItem> items { get; set; } = new();

    public Cart(Guid userId)
    {
        this.userId = userId;
    }

    public Cart() { }
}
