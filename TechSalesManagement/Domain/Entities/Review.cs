using System;
using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Domain.Entities;

public class Review : BaseEntity
{
    private Guid _userId;
    private Guid _productId;
    private int _rating;
    private string? _comment;
    private ReviewStatus _status;

    // Navigation Properties
    private User? _user;
    private Product? _product;
    private List<ReviewResponse> _responses = new();

    public Guid UserId
    {
        get => _userId;
        set => _userId = value;
    }

    public Guid ProductId
    {
        get => _productId;
        set => _productId = value;
    }

    public int Rating
    {
        get => _rating;
        set
        {
            if (value < 1) _rating = 1;
            else if (value > 5) _rating = 5;
            else _rating = value;
        }
    }

    public string? Comment
    {
        get => _comment;
        set => _comment = value;
    }

    public ReviewStatus Status
    {
        get => _status;
        set => _status = value;
    }

    public User? User
    {
        get => _user;
        set => _user = value;
    }

    public Product? Product
    {
        get => _product;
        set => _product = value;
    }

    public List<ReviewResponse> Responses
    {
        get => _responses;
        set => _responses = value ?? new();
    }

    public Review(Guid userId, Guid productId, int rating) : base()
    {
        UserId = userId;
        ProductId = productId;
        Rating = rating;
        Status = ReviewStatus.VISIBLE;
    }

    public Review() : base() { }
}
