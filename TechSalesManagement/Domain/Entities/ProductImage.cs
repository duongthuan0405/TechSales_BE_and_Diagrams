using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class ProductImage : BaseEntity
{
    public Guid product_id { get; set; }
    public string image_url { get; set; } = string.Empty;
    public bool is_primary { get; set; }

    public ProductImage(Guid productId, string imageUrl, bool isPrimary = false)
    {
        product_id = productId;
        image_url = imageUrl;
        is_primary = isPrimary;
    }

    public ProductImage() { }
}
