using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class ProductImage : BaseEntity
{
    private Guid _productId;
    private string _imageUrl = string.Empty;
    private bool _isPrimary;

    public Guid ProductId
    {
        get => _productId;
        set => _productId = value;
    }

    public string ImageUrl
    {
        get => _imageUrl;
        set => _imageUrl = value ?? string.Empty;
    }

    public bool IsPrimary
    {
        get => _isPrimary;
        set => _isPrimary = value;
    }

    public ProductImage(Guid productId, string imageUrl, bool isPrimary = false) : base()
    {
        ProductId = productId;
        ImageUrl = imageUrl;
        IsPrimary = isPrimary;
    }

    public ProductImage() : base() { }
}
