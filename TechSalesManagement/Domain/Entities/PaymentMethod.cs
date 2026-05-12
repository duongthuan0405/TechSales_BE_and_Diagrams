using System;
using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Domain.Entities;

public class PaymentMethod : BaseEntity
{
    public string name { get; set; } = string.Empty;
    public PaymentMethodType type { get; set; }

    public PaymentMethod(string name, PaymentMethodType type)
    {
        this.name = name;
        this.type = type;
    }

    public PaymentMethod() { }
}
