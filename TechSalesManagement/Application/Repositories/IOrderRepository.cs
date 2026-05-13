using System;
using System.Threading.Tasks;
using TechSalesManagement.Domain.Entities;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Application.Repositories;

public interface IOrderRepository
{
    Task AddOrderAsync(Order order, Guid? voucherId, Guid paymentMethodId);
    Task<(System.Collections.Generic.List<Order> orders, int totalCount)> GetOrdersByUserIdAsync(System.Guid userId, int pageNumber, int pageSize);
    Task<Order?> GetOrderDetailsByIdAsync(System.Guid orderId);
}
