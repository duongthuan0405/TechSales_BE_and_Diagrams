using System;
using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Domain.Entities;

public class Order
{
    public Guid id { get; set; }
    public DateTimeOffset createdAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? updatedAt { get; set; }

    public Guid userId { get; set; }
    public OrderStatus status { get; set; } = OrderStatus.PENDING;
    public decimal totalProductAmount { get; set; }
    public decimal shippingFee { get; set; }
    public decimal discountAmount { get; set; }
    public decimal totalAmount { get; set; }
    public string shippingAddressSnapshot { get; set; } = string.Empty;

    public List<OrderItem> items { get; set; } = new();
    public List<Voucher> vouchers { get; set; } = new();

    public Order(Guid userId, decimal totalAmount, string addressSnapshot)
    {
        this.userId = userId;
        this.totalAmount = totalAmount;
        this.shippingAddressSnapshot = addressSnapshot;
    }

    public Order() { }
}
