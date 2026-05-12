using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Domain.Entities;

public class PaymentMethod : BaseEntity
{
    private string _name = string.Empty;
    private PaymentMethodType _type;

    public string Name
    {
        get => _name;
        set => _name = value ?? string.Empty;
    }

    public PaymentMethodType Type
    {
        get => _type;
        set => _type = value;
    }

    public PaymentMethod(string name, PaymentMethodType type) : base()
    {
        Name = name;
        Type = type;
    }

    public PaymentMethod() : base() { }
}
