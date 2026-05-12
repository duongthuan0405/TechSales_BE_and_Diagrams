using System;
using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Domain.Entities;

public class Order : BaseEntity
{
    public Guid user_id { get; set; }
    public OrderStatus status { get; set; } = OrderStatus.PENDING;
    public decimal total_product_amount { get; set; }
    public decimal shipping_fee { get; set; }
    public decimal discount_amount { get; set; }
    public decimal total_amount { get; set; }
    public string shipping_address_snapshot { get; set; } = string.Empty;

    public List<OrderItem> items { get; set; } = new();
    public List<Voucher> vouchers { get; set; } = new();

    public Order(Guid userId, decimal totalAmount, string addressSnapshot)
    {
        user_id = userId;
        total_amount = totalAmount;
        shipping_address_snapshot = addressSnapshot;
    }

    public Order() { }
}
