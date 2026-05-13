using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechSalesManagement.Common;
using TechSalesManagement.Application.Services.Interfaces;
using TechSalesManagement.Application.Services.Params;
using TechSalesManagement.Presentation_WebAPI.DTOs.Common;
using TechSalesManagement.Presentation_WebAPI.DTOs.RequestDTOs;
using TechSalesManagement.Presentation_WebAPI.DTOs.ResponseDTOs;
using TechSalesManagement.Presentation_WebAPI.Extensions;

namespace TechSalesManagement.Presentation_WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/order")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiSuccessResponse<OrderResponseDto>>> PlaceOrderAsync([FromBody] PlaceOrderRequestDto request)
    {
        var userId = User.GetUserId();
        if (userId == null) return Unauthorized();

        var parameters = new PlaceOrderParams
        {
            UserId = userId.Value,
            ProductsWithQuantity = request.productsWithQuantity,
            ShippingAddressId = request.shippingAddressId,
            PaymentMethodId = request.paymentMethodId,
            VoucherCode = request.voucherCode
        };

        var newOrder = await _orderService.PlaceOrderAsync(parameters);

        var response = new OrderResponseDto
        {
            id = newOrder.id,
            status = newOrder.status,
            totalAmount = newOrder.totalAmount,
            createdAt = newOrder.createdAt
        };

        // BR91: Returns 200-OK with MSG37
        return Ok(new ApiSuccessResponse<OrderResponseDto>(response, MessageConstants.MSG37));
    }

    [HttpGet]
    public async Task<ActionResult<ApiSuccessResponse<PagedResponseDto<OrderResponseDto>>>> GetOrderHistoryAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userId = User.GetUserId();
        if (userId == null) return Unauthorized();

        var parameters = new GetOrderHistoryParams
        {
            UserId = userId.Value,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var (orders, totalCount) = await _orderService.GetOrderHistoryAsync(parameters);

        var items = orders.Select(o => new OrderResponseDto
        {
            id = o.id,
            status = o.status,
            totalAmount = o.totalAmount,
            createdAt = o.createdAt
        }).ToList();

        var response = new  PagedResponseDto<OrderResponseDto>
        {
            items = items,
            pageNumber = pageNumber,
            pageSize = pageSize,
            totalCount = totalCount
        };

        // BR108: Empty state returns MSG42
        string message = totalCount == 0 ? MessageConstants.MSG42 : "Order history retrieved successfully.";
        return Ok(new ApiSuccessResponse<PagedResponseDto<OrderResponseDto>>(response, message));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiSuccessResponse<OrderDetailResponseDto>>> GetOrderDetailsAsync([FromRoute] Guid id)
    {
        var userId = User.GetUserId();
        if (userId == null) return Unauthorized();

        var parameters = new GetOrderDetailsParams
        {
            OrderId = id,
            UserId = userId.Value
        };

        var order = await _orderService.GetOrderDetailsAsync(parameters);

        var response = new OrderDetailResponseDto
        {
            id = order.id,
            status = order.status,
            totalProductAmount = order.totalProductAmount,
            shippingFee = order.shippingFee,
            discountAmount = order.discountAmount,
            totalAmount = order.totalAmount,
            shippingAddressSnapshot = order.shippingAddressSnapshot,
            createdAt = order.createdAt,
            approvedAt = order.approvedAt,
            shippedAt = order.shippedAt,
            deliveredAt = order.deliveredAt,
            items = order.items.Select(i => new OrderItemResponseDto
            {
                productId = i.product_id,
                productName = i.product?.name ?? "Unknown Product",
                productImageUrl = i.product?.images?.FirstOrDefault(img => img.isPrimary)?.imageUrl ?? i.product?.images?.FirstOrDefault()?.imageUrl,
                price = i.price,
                quantity = i.quantity
            }).ToList()
        };

        return Ok(new ApiSuccessResponse<OrderDetailResponseDto>(response, "Order details retrieved successfully."));
    }

    [HttpPost("{id}/cancel")]
    public async Task<ActionResult<ApiSuccessResponse<object>>> CancelOrderAsync([FromRoute] Guid id)
    {
        var userId = User.GetUserId();
        if (userId == null) return Unauthorized();

        var parameters = new CancelOrderParams
        {
            OrderId = id,
            UserId = userId.Value
        };

        await _orderService.CancelOrderAsync(parameters);

        // BR122: Returns 200-OK with MSG46
        return Ok(new ApiSuccessResponse<object>(null, MessageConstants.MSG46));
    }
}
