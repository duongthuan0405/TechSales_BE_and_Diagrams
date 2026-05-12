using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class ReviewResponse : BaseEntity
{
    public Guid review_id { get; set; }
    public Guid user_id { get; set; }
    public string content { get; set; } = string.Empty;

    public ReviewResponse(Guid reviewId, Guid userId, string content)
    {
        review_id = reviewId;
        user_id = userId;
        this.content = content;
    }

    public ReviewResponse() { }
}
