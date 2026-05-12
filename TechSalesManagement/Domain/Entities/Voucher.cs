using System;
using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Domain.Entities;

public class Voucher : BaseEntity
{
    private string _code = string.Empty;
    private VoucherType _type;
    private decimal _value;
    private int? _maxUsage;
    private int _usedCount;
    private decimal _minOrderAmount;
    private DateTime? _startDate;
    private DateTime? _endDate;
    private bool _isActive;

    public string Code
    {
        get => _code;
        set => _code = value ?? string.Empty;
    }

    public VoucherType Type
    {
        get => _type;
        set => _type = value;
    }

    public decimal Value
    {
        get => _value;
        set => _value = value < 0 ? 0 : value;
    }

    public int? MaxUsage
    {
        get => _maxUsage;
        set => _maxUsage = value < 0 ? 0 : value;
    }

    public int UsedCount
    {
        get => _usedCount;
        set => _usedCount = value < 0 ? 0 : value;
    }

    public decimal MinOrderAmount
    {
        get => _minOrderAmount;
        set => _minOrderAmount = value < 0 ? 0 : value;
    }

    public DateTime? StartDate
    {
        get => _startDate;
        set => _startDate = value;
    }

    public DateTime? EndDate
    {
        get => _endDate;
        set => _endDate = value;
    }

    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    // Logic nghiệp vụ: Kiểm tra voucher còn dùng được không
    public bool CanBeUsed(decimal orderAmount)
    {
        if (!_isActive) return false;
        if (DateTime.UtcNow < _startDate || DateTime.UtcNow > _endDate) return false;
        if (_maxUsage.HasValue && _usedCount >= _maxUsage.Value) return false;
        if (orderAmount < _minOrderAmount) return false;
        
        return true;
    }

    public Voucher(string code, VoucherType type, decimal value) : base()
    {
        Code = code;
        Type = type;
        Value = value;
        IsActive = true;
    }

    public Voucher() : base() { }
}
