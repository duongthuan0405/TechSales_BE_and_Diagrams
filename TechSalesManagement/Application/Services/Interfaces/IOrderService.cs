using System.Threading.Tasks;
using TechSalesManagement.Application.Services.Params;
using TechSalesManagement.Domain.Entities;

namespace TechSalesManagement.Application.Services.Interfaces;

public interface IOrderService
{
    Task<Order> PlaceOrderAsync(PlaceOrderParams parameters);
    Task<(List<Order> orders, int totalCount)> GetOrderHistoryAsync(GetOrderHistoryParams parameters);
    Task<Order> GetOrderDetailsAsync(GetOrderDetailsParams parameters);
    Task CancelOrderAsync(CancelOrderParams parameters);
}
