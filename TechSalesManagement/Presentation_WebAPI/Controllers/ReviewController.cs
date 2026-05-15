using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechSalesManagement.Application.Services.Interfaces;
using TechSalesManagement.Presentation_WebAPI.DTOs.Common;
using TechSalesManagement.Presentation_WebAPI.DTOs.RequestDTOs;
using TechSalesManagement.Presentation_WebAPI.DTOs.ResponseDTOs;
using TechSalesManagement.Presentation_WebAPI.Extensions;
using TechSalesManagement.Common;

namespace TechSalesManagement.Presentation_WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [Authorize(Roles = "Staff,Admin")]
    [HttpGet("latest")]
    public async Task<ActionResult<ApiSuccessResponse<PagedResponseDto<ReviewStaffResponseDto>>>> GetLatestReviewsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var (reviews, totalCount) = await _reviewService.GetLatestReviewsAsync(pageNumber, pageSize);

        var response = new PagedResponseDto<ReviewStaffResponseDto>
        {
            items = reviews.Select(r => new ReviewStaffResponseDto
            {
                id = r.id,
                rating = r.rating,
                comment = r.comment,
                productName = r.productName,
                status = r.status.ToString(),
                violationReason = r.violationReason,
                createdAt = r.createdAt,
                profile = new ProfileResponseDto
                {
                    fullName = r.profile?.fullName ?? "Anonymous",
                    avatarUrl = r.profile?.avatarUrl
                }
            }).ToList(),
            totalCount = totalCount,
            pageNumber = pageNumber,
            pageSize = pageSize
        };

        return Ok(new ApiSuccessResponse<PagedResponseDto<ReviewStaffResponseDto>>(response, "Latest reviews retrieved successfully."));
    }

    [Authorize(Roles = "Staff,Admin")]
    [HttpPost("{id}/reply")]
    public async Task<ActionResult<ApiSuccessResponse<object>>> ReplyToReviewAsync([FromRoute] Guid id, [FromBody] ReviewReplyRequestDto request)
    {
        var staffId = User.GetUserId();
        if (staffId == null) return Unauthorized();

        await _reviewService.ReplyToReviewAsync(id, request.content, staffId.Value);

        return Ok(new ApiSuccessResponse<object>(null, MessageConstants.MSG67));
    }

    [Authorize(Roles = "Staff,Admin")]
    [HttpPut("{id}/hide")]
    public async Task<ActionResult<ApiSuccessResponse<object>>> HideReviewAsync([FromRoute] Guid id, [FromBody] ReviewHideRequestDto request)
    {
        var staffId = User.GetUserId();
        if (staffId == null) return Unauthorized();

        await _reviewService.HideReviewAsync(id, request.reason, staffId.Value);

        return Ok(new ApiSuccessResponse<object>(null, MessageConstants.MSG69));
    }
}
