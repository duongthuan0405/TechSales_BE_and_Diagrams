using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechSalesManagement.Application.Services.Interfaces;
using TechSalesManagement.Presentation_WebAPI.DTOs.Common;
using TechSalesManagement.Presentation_WebAPI.DTOs.ResponseDTOs;
using TechSalesManagement.Presentation_WebAPI.Extensions;
using TechSalesManagement.Domain.Enums;
using TechSalesManagement.Domain.Specifications;
using TechSalesManagement.Common;

namespace TechSalesManagement.Presentation_WebAPI.Controllers;

[ApiController]
[Route("api/admin/orders")]
[Authorize(Roles = "Staff,Business Admin,Technical Admin")]
public class OrderManagementController : ControllerBase
{
    private readonly IOrderManagementService _orderManagementService;

    public OrderManagementController(IOrderManagementService orderManagementService)
    {
        _orderManagementService = orderManagementService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiSuccessResponse<PagedResponseDto<OrderAdminSummaryDto>>>> GetOrdersAsync([FromQuery] OrderSearchParameters parameters)
    {
        var (items, totalCount) = await _orderManagementService.SearchOrdersAsync(parameters);
        
        var results = items.Select(i => {
            var firstPayment = i.payments.FirstOrDefault();
            return new OrderAdminSummaryDto
            {
                orderId = i.order.id,
                customerEmail = i.user?.email ?? "Unknown",
                customerName = i.user?.profile?.fullName ?? "Unknown",
                status = i.order.status,
                totalAmount = i.order.totalAmount,
                createdAt = i.order.createdAt,
                paymentMethodName = firstPayment.methodName ?? "Unknown",
                paymentStatus = firstPayment.payment?.status ?? PaymentStatus.PENDING
            };
        }).ToList();

        var response = new PagedResponseDto<OrderAdminSummaryDto>
        {
            items = results,
            totalCount = totalCount,
            pageNumber = parameters.PageNumber,
            pageSize = parameters.PageSize
        };

        return Ok(new ApiSuccessResponse<PagedResponseDto<OrderAdminSummaryDto>>(response, "Orders retrieved successfully."));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiSuccessResponse<object>>> GetDetailsAsync([FromRoute] Guid id)
    {
        var (order, user, payments) = await _orderManagementService.GetOrderDetailsAsync(id);
        
        var result = new
        {
            order = order,
            customer = user,
            payments = payments.Select(p => new { p.payment, p.methodName })
        };

        return Ok(new ApiSuccessResponse<object>(result, "Order details retrieved successfully."));
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<ApiSuccessResponse<object>>> UpdateStatusAsync([FromRoute] Guid id, [FromBody] UpdateOrderStatusRequestDto request)
    {
        var staffId = User.GetUserId();
        if (staffId == null) return Unauthorized();

        await _orderManagementService.UpdateOrderStatusAsync(id, request.status, staffId.Value);

        return Ok(new ApiSuccessResponse<object>(null, "Order status updated successfully."));
    }
}

public class OrderAdminSummaryDto
{
    public Guid orderId { get; set; }
    public string customerEmail { get; set; } = string.Empty;
    public string customerName { get; set; } = string.Empty;
    public OrderStatus status { get; set; }
    public decimal totalAmount { get; set; }
    public DateTimeOffset createdAt { get; set; }
    public string paymentMethodName { get; set; } = string.Empty;
    public PaymentStatus paymentStatus { get; set; }
}

public class UpdateOrderStatusRequestDto
{
    public OrderStatus status { get; set; }
}
