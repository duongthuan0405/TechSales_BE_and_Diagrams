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
[Route("api/review")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiSuccessResponse<object>>> AddReviewAsync([FromBody] AddReviewRequestDto request)
    {
        var userId = User.GetUserId();
        if (userId == null) return Unauthorized();

        var parameters = new AddReviewParams
        {
            UserId = userId.Value,
            OrderId = request.orderId,
            ProductId = request.productId,
            RatingStars = request.ratingStars,
            ReviewComment = request.reviewComment
        };

        await _reviewService.AddReviewAsync(parameters);

        // BR135: Returns 200-OK with success MSG50
        return Ok(new ApiSuccessResponse<object>(null, MessageConstants.MSG50));
    }

    [HttpGet("/api/product/{productId:guid}/reviews")]
    public async Task<ActionResult<ApiSuccessResponse<ProductReviewsResponseDto>>> GetProductReviewsAsync(
        [FromRoute] Guid productId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var parameters = new GetProductReviewsParams
        {
            ProductId = productId,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _reviewService.GetProductReviewsAsync(parameters);

        var response = new ProductReviewsResponseDto
        {
            averageRating = result.AverageRating,
            totalCount = result.TotalCount,
            pageNumber = pageNumber,
            pageSize = pageSize,
            items = result.Reviews.Select(r => new ReviewItemResponseDto
            {
                id = r.id,
                rating = r.rating,
                comment = r.comment,
                profile = new ProfileResponseDto
                {
                    fullName = r.profile?.fullName ?? "Anonymous Customer",
                    avatarUrl = r.profile?.avatarUrl,
                    phone = r.profile?.phone ?? string.Empty,
                    dateOfBirth = r.profile?.dateOfBirth
                },
                createdAt = r.createdAt
            }).ToList()
        };

        // BR140: Empty state displays MSG51
        string message = result.TotalCount == 0 ? MessageConstants.MSG51 : "Product reviews retrieved successfully.";
        return Ok(new ApiSuccessResponse<ProductReviewsResponseDto>(response, message));
    }
}
