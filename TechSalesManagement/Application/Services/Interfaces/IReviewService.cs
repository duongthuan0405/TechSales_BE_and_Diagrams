using System.Collections.Generic;
using System.Threading.Tasks;
using TechSalesManagement.Application.Services.Params;
using TechSalesManagement.Domain.Entities;

namespace TechSalesManagement.Application.Services.Interfaces;

public class ProductReviewsResult
{
    public List<Review> Reviews { get; set; } = new();
    public int TotalCount { get; set; }
    public decimal AverageRating { get; set; }
}

public interface IReviewService
{
    Task AddReviewAsync(AddReviewParams parameters);
    Task<ProductReviewsResult> GetProductReviewsAsync(GetProductReviewsParams parameters);
}
