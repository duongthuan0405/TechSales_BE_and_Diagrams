using System;

namespace TechSalesManagement.Domain.Entities;

public class Inventory
{
    public Guid product_id { get; set; }
    public int quantity { get; set; }
    public int reserved_quantity { get; set; }

    public int available_quantity => quantity - reserved_quantity;

    public Inventory(Guid productId, int quantity)
    {
        product_id = productId;
        this.quantity = quantity;
        reserved_quantity = 0;
    }

    public Inventory() { }
}
