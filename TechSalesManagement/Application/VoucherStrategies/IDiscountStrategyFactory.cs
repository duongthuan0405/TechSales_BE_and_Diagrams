using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Application.VoucherStrategies;

public interface IDiscountStrategyFactory
{
    IDiscountStrategy GetStrategy(VoucherType voucherType);
}
