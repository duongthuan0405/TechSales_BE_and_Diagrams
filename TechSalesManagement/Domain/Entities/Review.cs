using System;
using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Domain.Entities;

public class Review : BaseEntity
{
    public Guid? user_id { get; set; }
    public Guid product_id { get; set; }
    public int rating { get; set; }
    public string? comment { get; set; }
    public ReviewStatus status { get; set; } = ReviewStatus.VISIBLE;

    public Review(Guid? userId, Guid productId, int rating, string? comment)
    {
        user_id = userId;
        product_id = productId;
        this.rating = rating;
        this.comment = comment;
    }

    public Review() { }
}
