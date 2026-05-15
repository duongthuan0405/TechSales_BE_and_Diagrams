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
using TechSalesManagement.Domain.Entities;
using TechSalesManagement.Common;

namespace TechSalesManagement.Presentation_WebAPI.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Staff,Admin")]
public class AdminUserController : ControllerBase
{
    private readonly IAdminUserService _adminUserService;

    public AdminUserController(IAdminUserService adminUserService)
    {
        _adminUserService = adminUserService;
    }

    [HttpGet("customers")]
    public async Task<ActionResult<ApiSuccessResponse<PagedResponseDto<User>>>> GetCustomersAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var (items, totalCount) = await _adminUserService.GetCustomersAsync(pageNumber, pageSize);
        var response = new PagedResponseDto<User>
        {
            items = items,
            totalCount = totalCount,
            pageNumber = pageNumber,
            pageSize = pageSize
        };
        return Ok(new ApiSuccessResponse<PagedResponseDto<User>>(response, "Customer list retrieved successfully."));
    }

    [HttpPost("{id}/lock")]
    public async Task<ActionResult<ApiSuccessResponse<object>>> LockCustomerAsync([FromRoute] Guid id, [FromBody] LockUserRequestDto request)
    {
        var staffId = User.GetUserId();
        if (staffId == null) return Unauthorized();

        await _adminUserService.LockCustomerAsync(id, request.until, staffId.Value);

        return Ok(new ApiSuccessResponse<object>(null, "User locked successfully."));
    }

    [HttpPost("{id}/unlock")]
    public async Task<ActionResult<ApiSuccessResponse<object>>> UnlockCustomerAsync([FromRoute] Guid id)
    {
        var staffId = User.GetUserId();
        if (staffId == null) return Unauthorized();

        await _adminUserService.UnlockCustomerAsync(id, staffId.Value);

        return Ok(new ApiSuccessResponse<object>(null, "User unlocked successfully."));
    }
}

public class LockUserRequestDto
{
    public DateTimeOffset? until { get; set; }
}
