using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class Cart : BaseEntity
{
    public Guid user_id { get; set; }
    public List<CartItem> items { get; set; } = new();

    public Cart(Guid userId)
    {
        user_id = userId;
    }

    public Cart() { }
}
