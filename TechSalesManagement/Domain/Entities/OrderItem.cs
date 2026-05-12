using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class OrderItem : BaseEntity
{
    private Guid _orderId;
    private Guid _productId;
    private decimal _price;
    private int _quantity;

    // Navigation Properties
    private Product? _product;

    public Guid OrderId
    {
        get => _orderId;
        set => _orderId = value;
    }

    public Guid ProductId
    {
        get => _productId;
        set => _productId = value;
    }

    public decimal Price
    {
        get => _price;
        set => _price = value < 0 ? 0 : value;
    }

    public int Quantity
    {
        get => _quantity;
        set => _quantity = value < 1 ? 1 : value;
    }

    public Product? Product
    {
        get => _product;
        set => _product = value;
    }

    public decimal SubTotal => _price * _quantity;

    public OrderItem(Guid orderId, Guid productId, decimal price, int quantity) : base()
    {
        OrderId = orderId;
        ProductId = productId;
        Price = price;
        Quantity = quantity;
    }

    public OrderItem() : base() { }
}
