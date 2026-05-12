using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class CartItem : BaseEntity
{
    private Guid _cartId;
    private Guid _productId;
    private int _quantity;

    public Guid CartId
    {
        get => _cartId;
        set => _cartId = value;
    }

    public Guid ProductId
    {
        get => _productId;
        set => _productId = value;
    }

    public int Quantity
    {
        get => _quantity;
        set => _quantity = value < 1 ? 1 : value;
    }

    public CartItem(Guid cartId, Guid productId, int quantity) : base()
    {
        CartId = cartId;
        ProductId = productId;
        Quantity = quantity;
    }

    public CartItem() : base() { }
}
