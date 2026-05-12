using System;
using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Enums;
using TechSalesManagement.Domain.Constants;

namespace TechSalesManagement.Domain.Entities;

public class Product : BaseEntity
{
    private string _name = string.Empty;
    private string _description = string.Empty;
    private decimal _price;
    private ProductStatus _status;
    private string _brand = string.Empty;
    private Guid _categoryId;

    // Navigation Properties
    private Category? _category;
    private List<ProductImage> _images = new();

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(DomainErrors.Product.NameRequired);
            _name = value;
        }
    }

    public string Description
    {
        get => _description;
        set => _description = value ?? string.Empty;
    }

    public decimal Price
    {
        get => _price;
        set
        {
            if (value < 0)
                _price = 0;
            else
                _price = value;
        }
    }

    public ProductStatus Status
    {
        get => _status;
        set => _status = value;
    }

    public string Brand
    {
        get => _brand;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(DomainErrors.Product.BrandRequired);
            _brand = value;
        }
    }

    public Guid CategoryId
    {
        get => _categoryId;
        set => _categoryId = value;
    }

    public Category? Category
    {
        get => _category;
        set => _category = value;
    }

    public List<ProductImage> Images
    {
        get => _images;
        set => _images = value ?? new();
    }

    // Constructor cho logic nghiệp vụ
    public Product(string name, decimal price, string brand, Guid categoryId) : base()
    {
        Name = name;
        Price = price;
        Brand = brand;
        CategoryId = categoryId;
        Status = ProductStatus.ACTIVE;
    }

    // Constructor mặc định cho các trường hợp khác (như Mocking/Serialization)
    public Product() : base() { }
}
