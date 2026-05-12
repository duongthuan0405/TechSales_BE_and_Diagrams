using System;
using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Domain.Entities;

public class Payment : BaseEntity
{
    private Guid _orderId;
    private Guid _paymentMethodId;
    private PaymentStatus _status;
    private decimal _amount;
    private string? _transactionRef;

    // Navigation Properties
    private Order? _order;
    private PaymentMethod? _paymentMethod;

    public Guid OrderId
    {
        get => _orderId;
        set => _orderId = value;
    }

    public Guid PaymentMethodId
    {
        get => _paymentMethodId;
        set => _paymentMethodId = value;
    }

    public PaymentStatus Status
    {
        get => _status;
        set => _status = value;
    }

    public decimal Amount
    {
        get => _amount;
        set => _amount = value < 0 ? 0 : value;
    }

    public string? TransactionRef
    {
        get => _transactionRef;
        set => _transactionRef = value;
    }

    public Order? Order
    {
        get => _order;
        set => _order = value;
    }

    public PaymentMethod? PaymentMethod
    {
        get => _paymentMethod;
        set => _paymentMethod = value;
    }

    public Payment(Guid orderId, Guid paymentMethodId, decimal amount) : base()
    {
        OrderId = orderId;
        PaymentMethodId = paymentMethodId;
        Amount = amount;
        Status = PaymentStatus.PENDING;
    }

    public Payment() : base() { }
}
