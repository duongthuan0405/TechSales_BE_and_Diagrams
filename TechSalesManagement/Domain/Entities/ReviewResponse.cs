using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Constants;

namespace TechSalesManagement.Domain.Entities;

public class ReviewResponse : BaseEntity
{
    private Guid _reviewId;
    private Guid _userId;
    private string _content = string.Empty;

    public Guid ReviewId
    {
        get => _reviewId;
        set => _reviewId = value;
    }

    public Guid UserId
    {
        get => _userId; // Người phản hồi (thường là Staff)
        set => _userId = value;
    }

    public string Content
    {
        get => _content;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(DomainErrors.ReviewResponse.ContentRequired);
            _content = value;
        }
    }

    public ReviewResponse(Guid reviewId, Guid userId, string content) : base()
    {
        ReviewId = reviewId;
        UserId = userId;
        Content = content;
    }

    public ReviewResponse() : base() { }
}
