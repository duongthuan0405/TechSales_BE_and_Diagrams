using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Constants;

namespace TechSalesManagement.Domain.Entities;

public class Role : BaseEntity
{
    private string _name = string.Empty;
    private string _description = string.Empty;
    private List<Permission> _permissions = new();

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(DomainErrors.Role.NameRequired);
            _name = value;
        }
    }

    public string Description
    {
        get => _description;
        set => _description = value ?? string.Empty;
    }

    public List<Permission> Permissions
    {
        get => _permissions;
        set => _permissions = value ?? new();
    }

    public Role(string name, string description) : base()
    {
        Name = name;
        Description = description;
    }

    public Role() : base() { }
}
