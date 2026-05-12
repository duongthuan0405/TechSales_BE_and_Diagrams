using System;
using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Domain.Entities;

public class Payment : BaseEntity
{
    public Guid order_id { get; set; }
    public Guid payment_method_id { get; set; }
    public PaymentStatus status { get; set; } = PaymentStatus.PENDING;
    public decimal amount { get; set; }
    public string? transaction_ref { get; set; }

    public Payment(Guid orderId, Guid paymentMethodId, decimal amount)
    {
        order_id = orderId;
        payment_method_id = paymentMethodId;
        this.amount = amount;
    }

    public Payment() { }
}
