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

namespace TechSalesManagement.Application.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IVoucherRepository _voucherRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IShippingAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        IVoucherRepository voucherRepository,
        IInventoryRepository inventoryRepository,
        IShippingAddressRepository addressRepository,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _voucherRepository = voucherRepository;
        _inventoryRepository = inventoryRepository;
        _addressRepository = addressRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Order> PlaceOrderAsync(PlaceOrderParams parameters)
    {
        // 1. Check empty selection (BR74)
        if (parameters.ProductsWithQuantity == null || !parameters.ProductsWithQuantity.Any())
        {
            throw new BadRequestException(MessageConstants.MSG32);
        }

        // 4. Verify Stock & Real-time Calculations (BR86 / BR87)
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
                // BR87: Insufficient stock MSG36
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

        // 5. Extract and Build Address Snapshot for Order history persistence
        var address = await _addressRepository.GetByIdAsync(parameters.ShippingAddressId);
        if (address == null || address.userId != parameters.UserId)
        {
            throw new BadRequestException("Selected shipping address is invalid.");
        }
        string addressSnapshot = $"{address.detail}, {address.ward}, {address.province}";

        // 6. Voucher Verification & Discount Logic (BR78 / BR79 / BR81)
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

            // Calculate discount values
            if (appliedVoucher.type == VoucherType.FIXED)
            {
                discountAmount = Math.Min(appliedVoucher.value, totalProductAmount);
            }
            else if (appliedVoucher.type == VoucherType.PERCENT)
            {
                discountAmount = totalProductAmount * (appliedVoucher.value / 100m);
            }
        }

        // 7. Total calculations
        decimal shippingFee = 0; // Standard zero-fee fallback or configurable value
        decimal totalAmount = totalProductAmount + shippingFee - discountAmount;

        // 8. Model domain order mapping
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

        // 9. ATOMIC TRANSACTIONS (Save Order, Deduct Stock, Clear Cart, Increment Voucher usage)
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

        // Global atomic save via unit of work
        await _unitOfWork.FinishAsync();

        return newOrder;
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
}
