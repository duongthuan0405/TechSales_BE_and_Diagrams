using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSalesManagement.Application.Exceptions;
using TechSalesManagement.Application.HelperServices;
using TechSalesManagement.Application.Interfaces;
using TechSalesManagement.Application.Repositories;
using TechSalesManagement.Application.Services.Interfaces;
using TechSalesManagement.Domain.Entities;
using TechSalesManagement.Domain.Enums;
using TechSalesManagement.Domain.Specifications;

namespace TechSalesManagement.Application.Services.Implementations;

public class OrderManagementService : IOrderManagementService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public OrderManagementService(
        IOrderRepository orderRepository,
        IInventoryRepository inventoryRepository,
        IUserRepository userRepository,
        IAuditLogRepository auditLogRepository,
        IEmailService emailService,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _inventoryRepository = inventoryRepository;
        _userRepository = userRepository;
        _auditLogRepository = auditLogRepository;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
    }

    public async Task<(List<(Order order, User? user)> items, int totalCount)> SearchOrdersAsync(OrderSearchParameters parameters)
    {
        return await _orderRepository.SearchOrdersAsync(parameters);
    }

    public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus nextStatus, Guid staffId)
    {
        var result = await _orderRepository.GetOrderWithFullDetailsByIdAsync(orderId);
        if (result == null) throw new NotFoundException("Order not found.");

        var order = result.Value.order;
        if (order == null) throw new NotFoundException("Order not found.");

        var oldStatus = order.status;
        if (oldStatus == nextStatus) return;

        try
        {
            await _unitOfWork.BeginAsync();

            // Transition logic
            switch (nextStatus)
            {
                case OrderStatus.APPROVED:
                    order.Approve();
                    break;
                case OrderStatus.SHIPPING:
                    order.Ship();
                    break;
                case OrderStatus.DELIVERED:
                    order.Deliver();
                    break;
                case OrderStatus.CANCELLED:
                    order.Cancel();
                    // Restock logic
                    foreach (var item in order.items)
                    {
                        await _inventoryRepository.ReleaseStockAsync(item.product_id, item.quantity);
                    }
                    break;
                default:
                    throw new BadRequestException("Invalid status transition.");
            }

            await _orderRepository.UpdateOrderAsync(order);

            var auditLog = new AuditLog(staffId, "UPDATE_ORDER_STATUS", "Orders", $"OrderId: {orderId}, From: {oldStatus}, To: {nextStatus}");
            await _auditLogRepository.AddAsync(auditLog);

            await _unitOfWork.FinishAsync();

            // Notify Customer (Best effort, outside transaction)
            try
            {
                var user = await _userRepository.GetByIdAsync(order.userId);
                if (user != null && !string.IsNullOrEmpty(user.email))
                {
                    await _emailService.SendOrderConfirmationEmailAsync(user.email, order.id, order.totalAmount, "Status updated to: " + nextStatus.ToString());
                }
            }
            catch { /* Log error but don't fail */ }
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<(Order? order, User? user, List<(Payment payment, string methodName)> payments)> GetOrderDetailsAsync(Guid orderId)
    {
        var result = await _orderRepository.GetOrderWithFullDetailsByIdAsync(orderId);
        if (result == null) throw new NotFoundException("Order not found.");

        return (result.Value.order, result.Value.user, result.Value.payments);
    }
}
