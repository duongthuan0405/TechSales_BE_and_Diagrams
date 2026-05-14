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
    Task CancelOrderAsync(System.Guid orderId);

    // Staff methods
    Task<(System.Collections.Generic.List<(Order order, User? user)> orders, int totalCount)> GetOrdersByStatusAsync(OrderStatus status, int pageNumber, int pageSize);
    Task<(Order? order, User? user, System.Collections.Generic.List<(Payment payment, string methodName)> payments)?> GetOrderWithFullDetailsByIdAsync(System.Guid orderId);
    Task UpdateStatusAsync(System.Guid orderId, OrderStatus status);
}
