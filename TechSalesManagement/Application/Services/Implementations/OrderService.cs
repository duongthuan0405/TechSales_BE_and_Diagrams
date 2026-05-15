using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechSalesManagement.Application.Exceptions;
using TechSalesManagement.Application.Interfaces;
using TechSalesManagement.Application.Repositories;
using TechSalesManagement.Application.Services.Interfaces;
using TechSalesManagement.Application.Services.Params;
using TechSalesManagement.Common;
using TechSalesManagement.Domain.Entities;
using TechSalesManagement.Domain.Enums;
using TechSalesManagement.Application.HelperServices;
using TechSalesManagement.Application.VoucherStrategies;
using TechSalesManagement.Application.Services.Strategies.Refund;

namespace TechSalesManagement.Application.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IVoucherRepository _voucherRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IShippingAddressRepository _addressRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IDiscountStrategyFactory _discountStrategyFactory;
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IRefundStrategyFactory _refundStrategyFactory;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        IVoucherRepository voucherRepository,
        IInventoryRepository inventoryRepository,
        IShippingAddressRepository addressRepository,
        IUserRepository userRepository,
        IEmailService emailService,
        IDiscountStrategyFactory discountStrategyFactory,
        IAuditLogRepository auditLogRepository,
        IRefundStrategyFactory refundStrategyFactory,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _voucherRepository = voucherRepository;
        _inventoryRepository = inventoryRepository;
        _addressRepository = addressRepository;
        _userRepository = userRepository;
        _emailService = emailService;
        _discountStrategyFactory = discountStrategyFactory;
        _auditLogRepository = auditLogRepository;
        _refundStrategyFactory = refundStrategyFactory;
        _unitOfWork = unitOfWork;
    }

    public async Task<Order> PlaceOrderAsync(PlaceOrderParams parameters)
    {
        if (parameters.ProductsWithQuantity == null || !parameters.ProductsWithQuantity.Any())
        {
            throw new BadRequestException(MessageConstants.MSG32);
        }

        if (parameters.ShippingAddressId == Guid.Empty)
        {
            throw new BadRequestException("Shipping address is required.");
        }

        if (parameters.PaymentMethodId == Guid.Empty)
        {
            throw new BadRequestException("Payment method is required.");
        }

        try
        {
            await _unitOfWork.BeginAsync();

            decimal totalProductAmount = 0;
            var orderItems = new List<OrderItem>();
            var orderId = Guid.NewGuid();

            foreach (var kvp in parameters.ProductsWithQuantity)
            {
                var productId = kvp.Key;
                var requestedQty = kvp.Value;

                if (requestedQty <= 0)
                {
                    throw new BadRequestException(MessageConstants.MSG29);
                }

                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    throw new BadRequestException(MessageConstants.MSG25);
                }

                var availableQty = product.inventory?.availableQuantity ?? 0;
                if (requestedQty > availableQty)
                {
                    throw new BadRequestException(MessageConstants.MSG36);
                }

                totalProductAmount += product.price * requestedQty;

                orderItems.Add(new OrderItem
                {
                    order_id = orderId,
                    product_id = productId,
                    price = product.price,
                    quantity = requestedQty
                });
            }

            var address = await _addressRepository.GetByIdAsync(parameters.ShippingAddressId);
            if (address == null || address.userId != parameters.UserId)
            {
                throw new BadRequestException("Selected shipping address is invalid.");
            }
            string addressSnapshot = $"{address.detail}, {address.ward}, {address.province}";

            Voucher? appliedVoucher = null;
            decimal discountAmount = 0;

            if (!string.IsNullOrWhiteSpace(parameters.VoucherCode))
            {
                appliedVoucher = await _voucherRepository.GetByCodeAsync(parameters.VoucherCode);
                
                if (appliedVoucher == null || !appliedVoucher.isActive)
                {
                    throw new BadRequestException(MessageConstants.MSG33);
                }

                var now = DateTimeOffset.UtcNow;
                if ((appliedVoucher.startDate.HasValue && appliedVoucher.startDate.Value > now) ||
                    (appliedVoucher.endDate.HasValue && appliedVoucher.endDate.Value < now))
                {
                    throw new BadRequestException(MessageConstants.MSG33);
                }

                if (appliedVoucher.maxUsage > 0 && appliedVoucher.usedCount >= appliedVoucher.maxUsage)
                {
                    throw new BadRequestException(MessageConstants.MSG33);
                }

                if (totalProductAmount < appliedVoucher.minOrderAmount)
                {
                    throw new BadRequestException(MessageConstants.MSG33);
                }

                var strategy = _discountStrategyFactory.GetStrategy(appliedVoucher.type);
                discountAmount = strategy.CalculateDiscount(totalProductAmount, appliedVoucher.value);
            }

            decimal shippingFee = 0;
            decimal totalAmount = totalProductAmount + shippingFee - discountAmount;

            var newOrder = new Order
            {
                id = orderId,
                userId = parameters.UserId,
                status = OrderStatus.PENDING,
                totalProductAmount = totalProductAmount,
                shippingFee = shippingFee,
                discountAmount = discountAmount,
                totalAmount = totalAmount,
                shippingAddressSnapshot = addressSnapshot,
                createdAt = DateTimeOffset.UtcNow,
                items = orderItems
            };

            await _orderRepository.AddOrderAsync(newOrder, appliedVoucher?.id, parameters.PaymentMethodId);

            foreach (var item in orderItems)
            {
                await _inventoryRepository.ReserveStockAsync(item.product_id, item.quantity);
            }

            var cart = await _cartRepository.GetByUserIdAsync(parameters.UserId);
            if (cart != null)
            {
                foreach (var kvp in parameters.ProductsWithQuantity)
                {
                    await _cartRepository.RemoveItemAsync(cart.id, kvp.Key);
                }
            }

            if (appliedVoucher != null)
            {
                appliedVoucher.usedCount += 1;
                await _voucherRepository.UpdateVoucherAsync(appliedVoucher);
            }

            await _unitOfWork.FinishAsync();

            try
            {
                var user = await _userRepository.GetByIdAsync(parameters.UserId);
                if (user != null && !string.IsNullOrEmpty(user.email))
                {
                    await _emailService.SendOrderConfirmationEmailAsync(user.email, newOrder.id, newOrder.totalAmount, newOrder.shippingAddressSnapshot);
                }
            }
            catch (Exception)
            {
            }

            return newOrder;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<(List<Order> orders, int totalCount)> GetOrderHistoryAsync(GetOrderHistoryParams parameters)
    {
        if (parameters.PageNumber <= 0) parameters.PageNumber = 1;
        if (parameters.PageSize <= 0) parameters.PageSize = 10;

        return await _orderRepository.GetOrdersByUserIdAsync(parameters.UserId, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<Order> GetOrderDetailsAsync(GetOrderDetailsParams parameters)
    {
        var order = await _orderRepository.GetOrderDetailsByIdAsync(parameters.OrderId);

        if (order == null || order.userId != parameters.UserId)
        {
            throw new NotFoundException(MessageConstants.MSG43);
        }

        return order;
    }

    public async Task CancelOrderAsync(CancelOrderParams parameters)
    {
        if (parameters.OrderId == Guid.Empty)
        {
            throw new BadRequestException("Order Id is required.");
        }

        var order = await _orderRepository.GetOrderDetailsByIdAsync(parameters.OrderId);

        if (order == null || order.userId != parameters.UserId)
        {
            throw new NotFoundException(MessageConstants.MSG43);
        }

        if (order.status != OrderStatus.PENDING)
        {
            throw new BadRequestException(MessageConstants.MSG45);
        }

        try
        {
            await _unitOfWork.BeginAsync();

            await _orderRepository.CancelOrderAsync(order.id);

            if (order.items != null && order.items.Any())
            {
                foreach (var item in order.items)
                {
                    await _inventoryRepository.ReleaseStockAsync(item.product_id, item.quantity);
                }
            }

            if (order.vouchers != null && order.vouchers.Any())
            {
                foreach (var voucher in order.vouchers)
                {
                    voucher.usedCount = Math.Max(0, voucher.usedCount - 1);
                    await _voucherRepository.UpdateVoucherAsync(voucher);
                }
            }

            await _unitOfWork.FinishAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    // Staff methods
    public async Task<(List<(Order order, User? user)> orders, int totalCount)> GetPendingOrdersAsync(GetPendingOrdersParams parameters)
    {
        if (parameters.PageNumber <= 0) parameters.PageNumber = 1;
        if (parameters.PageSize <= 0) parameters.PageSize = 20;

        return await _orderRepository.GetOrdersByStatusAsync(OrderStatus.PENDING, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<(Order order, User? user, List<(Payment payment, string methodName)> payments)> GetOrderWithFullDetailsAsync(Guid orderId)
    {
        var result = await _orderRepository.GetOrderWithFullDetailsByIdAsync(orderId);

        if (result == null || result.Value.order == null)
        {
            throw new NotFoundException(MessageConstants.MSG43);
        }

        return (result.Value.order, result.Value.user, result.Value.payments);
    }

    public async Task ApproveOrderAsync(ApproveOrderParams parameters)
    {
        var order = await _orderRepository.GetOrderDetailsByIdAsync(parameters.OrderId);

        if (order == null)
        {
            throw new NotFoundException(MessageConstants.MSG43);
        }

        if (order.status != OrderStatus.PENDING)
        {
            throw new BadRequestException("Only pending orders can be approved.");
        }

        try
        {
            await _unitOfWork.BeginAsync();

            await _orderRepository.UpdateStatusAsync(parameters.OrderId, OrderStatus.APPROVED);

            var auditLog = new AuditLog(
                parameters.StaffId,
                "APPROVE_ORDER",
                "Orders",
                parameters.OrderId.ToString()
            );
            await _auditLogRepository.AddAsync(auditLog);

            await _unitOfWork.FinishAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task ShipOrderAsync(Guid orderId, Guid staffId)
    {
        var order = await _orderRepository.GetOrderDetailsByIdAsync(orderId);

        if (order == null)
        {
            throw new NotFoundException(MessageConstants.MSG43);
        }

        if (order.status != OrderStatus.APPROVED)
        {
            throw new BadRequestException("Only approved orders can be shipped.");
        }

        try
        {
            await _unitOfWork.BeginAsync();

            await _orderRepository.UpdateStatusAsync(orderId, OrderStatus.SHIPPING);

            var auditLog = new AuditLog(
                staffId,
                "SHIP_ORDER",
                "Orders",
                orderId.ToString()
            );
            await _auditLogRepository.AddAsync(auditLog);

            await _unitOfWork.FinishAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task ConfirmDeliveryAsync(Guid orderId, Guid staffId)
    {
        var order = await _orderRepository.GetOrderDetailsByIdAsync(orderId);

        if (order == null)
        {
            throw new NotFoundException(MessageConstants.MSG43);
        }

        if (order.status != OrderStatus.SHIPPING)
        {
            throw new BadRequestException("Only shipping orders can be confirmed as delivered.");
        }

        try
        {
            await _unitOfWork.BeginAsync();

            await _orderRepository.UpdateStatusAsync(orderId, OrderStatus.DELIVERED);

            var auditLog = new AuditLog(
                staffId,
                "DELIVER_ORDER",
                "Orders",
                orderId.ToString()
            );
            await _auditLogRepository.AddAsync(auditLog);

            await _unitOfWork.FinishAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task StaffCancelOrderAsync(Guid orderId, Guid staffId, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new BadRequestException("Reason for cancellation is required.");
        }

        var order = await _orderRepository.GetOrderDetailsByIdAsync(orderId);

        if (order == null)
        {
            throw new NotFoundException(MessageConstants.MSG43);
        }

        if (order.status == OrderStatus.DELIVERED)
        {
            throw new BadRequestException("Cannot cancel a delivered order.");
        }

        try
        {
            await _unitOfWork.BeginAsync();

            await _orderRepository.UpdateStatusAsync(orderId, OrderStatus.CANCELLED);

            if (order.items != null && order.items.Any())
            {
                foreach (var item in order.items)
                {
                    await _inventoryRepository.ReleaseStockAsync(item.product_id, item.quantity);
                }
            }

            var auditLog = new AuditLog(
                staffId,
                "CANCEL_ORDER",
                "Orders",
                $"{orderId} - Reason: {reason}"
            );
            await _auditLogRepository.AddAsync(auditLog);

            await _unitOfWork.FinishAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task InitiateRefundAsync(Guid orderId, Guid staffId)
    {
        var result = await _orderRepository.GetOrderWithFullDetailsByIdAsync(orderId);

        if (result == null || result.Value.order == null)
        {
            throw new NotFoundException(MessageConstants.MSG43);
        }

        var order = result.Value.order;
        var payments = result.Value.payments;

        if (order.status != OrderStatus.CANCELLED)
        {
            throw new BadRequestException(MessageConstants.MSG64);
        }

        var successfulPayment = payments.FirstOrDefault(p => p.payment.status == PaymentStatus.SUCCESS);
        if (successfulPayment.payment == null)
        {
            throw new BadRequestException("Order has no completed payment to refund.");
        }

        try
        {
            await _unitOfWork.BeginAsync();

            var methodType = string.IsNullOrEmpty(successfulPayment.payment.transactionRef) 
                ? PaymentMethodType.CASH 
                : PaymentMethodType.ONLINE;

            var strategy = _refundStrategyFactory.GetStrategy(methodType);
            bool success = await strategy.ExecuteRefundAsync(successfulPayment.payment);

            if (success)
            {
                // Update payment status (assuming we have a method for this)
                // In a real project, we'd add UpdatePaymentStatusAsync to Repository
                
                var auditLog = new AuditLog(
                    staffId,
                    "INITIATE_REFUND",
                    "Orders",
                    $"{orderId} - Amount: {successfulPayment.payment.amount}"
                );
                await _auditLogRepository.AddAsync(auditLog);

                await _unitOfWork.FinishAsync();
            }
            else
            {
                throw new BadRequestException("Refund failed through payment gateway.");
            }
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<(List<(Order order, User? user, List<Payment> payments)> orders, int totalCount)> GetRefundRequestsAsync(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0) pageNumber = 1;
        if (pageSize <= 0) pageSize = 20;

        return await _orderRepository.GetRefundableOrdersAsync(pageNumber, pageSize);
    }

    public async Task<(List<(Order order, User? user)> orders, int totalCount)> SearchOrdersAsync(TechSalesManagement.Domain.Specifications.OrderSearchParameters parameters)
    {
        return await _orderRepository.SearchOrdersAsync(parameters);
    }
}
