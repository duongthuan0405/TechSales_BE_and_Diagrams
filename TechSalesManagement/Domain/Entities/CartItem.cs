using System;

namespace TechSalesManagement.Domain.Entities;

public class CartItem
{
    public Guid cart_id { get; set; }
    public Guid product_id { get; set; }
    public int quantity { get; set; }
    public DateTimeOffset created_at { get; set; }
    public DateTimeOffset updated_at { get; set; }

    public CartItem(Guid cartId, Guid productId, int quantity)
    {
        cart_id = cartId;
        product_id = productId;
        this.quantity = quantity;
        created_at = DateTimeOffset.UtcNow;
        updated_at = DateTimeOffset.UtcNow;
    }

    public CartItem() { }
}
