using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Constants;

namespace TechSalesManagement.Domain.Entities;

public class Category : BaseEntity
{
    private string _name = string.Empty;
    private List<Product> _products = new();

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(DomainErrors.Category.NameRequired);
            _name = value;
        }
    }

    public List<Product> Products
    {
        get => _products;
        set => _products = value ?? new();
    }

    public Category(string name) : base()
    {
        Name = name;
    }

    public Category() : base() { }
}
