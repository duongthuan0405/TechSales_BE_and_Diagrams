using System;
using System.Collections.Generic;
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

    // Staff methods
    Task<(List<(Order order, User? user)> orders, int totalCount)> GetPendingOrdersAsync(GetPendingOrdersParams parameters);
    Task<(Order order, User? user, List<(Payment payment, string methodName)> payments)> GetOrderWithFullDetailsAsync(Guid orderId);
    Task ApproveOrderAsync(ApproveOrderParams parameters);
}
