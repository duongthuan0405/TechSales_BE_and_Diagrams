using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSalesManagement.Application.Services.Params;
using TechSalesManagement.Domain.Entities;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Application.Services.Interfaces;

public interface IOrderService
{
    Task<Order> PlaceOrderAsync(PlaceOrderParams parameters);
    Task<(List<(Order order, List<(Payment payment, string methodName, PaymentMethodType type)> payments)> orders, int totalCount)> GetOrderHistoryAsync(GetOrderHistoryParams parameters);
    Task<Order> GetOrderDetailsAsync(GetOrderDetailsParams parameters);
    Task CancelOrderAsync(CancelOrderParams parameters);

    // Staff methods
    Task<(List<(Order order, User? user, List<(Payment payment, string methodName, PaymentMethodType type)> payments)> orders, int totalCount)> GetPendingOrdersAsync(GetPendingOrdersParams parameters);
    Task<(Order order, User? user, List<(Payment payment, string methodName, PaymentMethodType type)> payments)> GetOrderWithFullDetailsAsync(Guid orderId);
    Task ApproveOrderAsync(ApproveOrderParams parameters);
    Task ShipOrderAsync(Guid orderId, Guid staffId);
    Task ConfirmDeliveryAsync(Guid orderId, Guid staffId);
    Task StaffCancelOrderAsync(Guid orderId, Guid staffId, string reason);
    Task InitiateRefundAsync(Guid orderId, Guid staffId);
    Task<(List<(Order order, User? user, List<(Payment payment, string methodName, PaymentMethodType type)> payments)> orders, int totalCount)> GetRefundRequestsAsync(int pageNumber, int pageSize);
    Task<(List<(Order order, User? user, List<(Payment payment, string methodName, PaymentMethodType type)> payments)> orders, int totalCount)> SearchOrdersAsync(TechSalesManagement.Domain.Specifications.OrderSearchParameters parameters);
}
