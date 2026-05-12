using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class ShippingAddress : BaseEntity
{
    private Guid _userId;
    private string _province = string.Empty;
    private string _ward = string.Empty;
    private string _detail = string.Empty;
    private bool _isDefault;

    public Guid UserId
    {
        get => _userId;
        set => _userId = value;
    }

    public string Province
    {
        get => _province;
        set => _province = value ?? string.Empty;
    }

    public string Ward
    {
        get => _ward;
        set => _ward = value ?? string.Empty;
    }

    public string Detail
    {
        get => _detail;
        set => _detail = value ?? string.Empty;
    }

    public bool IsDefault
    {
        get => _isDefault;
        set => _isDefault = value;
    }

    public ShippingAddress(Guid userId, string province, string ward, string detail) : base()
    {
        UserId = userId;
        Province = province;
        Ward = ward;
        Detail = detail;
    }

    public ShippingAddress() : base() { }
}
