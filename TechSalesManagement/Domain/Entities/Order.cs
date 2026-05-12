using System;
using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Domain.Entities;

public class Order : BaseEntity
{
    private Guid _userId;
    private OrderStatus _status;
    private decimal _totalProductAmount;
    private decimal _shippingFee;
    private decimal _discountAmount;
    private decimal _totalAmount;
    private string _shippingAddressSnapshot = string.Empty;

    // Navigation Properties
    private User? _user;
    private List<OrderItem> _items = new();
    private List<Payment> _payments = new();

    public Guid UserId
    {
        get => _userId;
        set => _userId = value;
    }

    public OrderStatus Status
    {
        get => _status;
        set => _status = value;
    }

    public decimal TotalProductAmount
    {
        get => _totalProductAmount;
        set => _totalProductAmount = value < 0 ? 0 : value;
    }

    public decimal ShippingFee
    {
        get => _shippingFee;
        set => _shippingFee = value < 0 ? 0 : value;
    }

    public decimal DiscountAmount
    {
        get => _discountAmount;
        set => _discountAmount = value < 0 ? 0 : value;
    }

    public decimal TotalAmount
    {
        get => _totalAmount;
        set => _totalAmount = value < 0 ? 0 : value;
    }

    public string ShippingAddressSnapshot
    {
        get => _shippingAddressSnapshot;
        set => _shippingAddressSnapshot = value ?? string.Empty;
    }

    public User? User
    {
        get => _user;
        set => _user = value;
    }

    public List<OrderItem> Items
    {
        get => _items;
        set => _items = value ?? new();
    }

    public List<Payment> Payments
    {
        get => _payments;
        set => _payments = value ?? new();
    }

    public Order(Guid userId, string addressSnapshot) : base()
    {
        UserId = userId;
        ShippingAddressSnapshot = addressSnapshot;
        Status = OrderStatus.PENDING;
        TotalProductAmount = 0;
        ShippingFee = 0;
        DiscountAmount = 0;
        TotalAmount = 0;
    }

    public Order() : base() { }

    // Logic nghiệp vụ: Tính toán lại tổng tiền cuối cùng
    public void RecalculateTotal()
    {
        TotalAmount = TotalProductAmount + ShippingFee - DiscountAmount;
    }
}
