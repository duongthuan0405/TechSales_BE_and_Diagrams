namespace TechSalesManagement.Application.VoucherStrategies;

public interface IDiscountStrategy
{
    decimal CalculateDiscount(decimal totalAmount, decimal voucherValue);
}
