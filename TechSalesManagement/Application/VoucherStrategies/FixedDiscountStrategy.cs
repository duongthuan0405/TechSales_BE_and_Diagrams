using System;

namespace TechSalesManagement.Application.VoucherStrategies;

public class FixedDiscountStrategy : IDiscountStrategy
{
    public decimal CalculateDiscount(decimal totalAmount, decimal voucherValue)
    {
        return Math.Min(voucherValue, totalAmount);
    }
}
