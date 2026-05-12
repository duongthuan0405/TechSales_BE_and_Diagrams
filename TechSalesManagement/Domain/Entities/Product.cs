using System;
using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Domain.Entities;

public class Product : BaseEntity
{
    public string name { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public decimal price { get; set; }
    public ProductStatus status { get; set; } = ProductStatus.ACTIVE;
    public string brand { get; set; } = string.Empty;
    public Guid category_id { get; set; }

    public List<ProductImage> images { get; set; } = new();

    public Product(string name, decimal price, Guid categoryId)
    {
        this.name = name;
        this.price = price;
        category_id = categoryId;
    }

    public Product() { }
}
