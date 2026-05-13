using System;
using System.Collections.Generic;

namespace TechSalesManagement.Presentation_WebAPI.DTOs.ResponseDTOs;

public class ReviewItemResponseDto
{
    public Guid id { get; set; }
    public int rating { get; set; }
    public string? comment { get; set; }
    public ProfileResponseDto profile { get; set; } = null!;
    public DateTimeOffset createdAt { get; set; }
}

public class ProductReviewsResponseDto
{
    public decimal averageRating { get; set; }
    public int totalCount { get; set; }
    public int pageNumber { get; set; }
    public int pageSize { get; set; }
    public List<ReviewItemResponseDto> items { get; set; } = new();
}
