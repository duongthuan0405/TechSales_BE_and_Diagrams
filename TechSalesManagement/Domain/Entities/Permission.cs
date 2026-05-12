using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Constants;

namespace TechSalesManagement.Domain.Entities;

public class Permission : BaseEntity
{
    private string _code = string.Empty;
    private string _name = string.Empty;
    private string _module = string.Empty;

    public string Code
    {
        get => _code;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(DomainErrors.Permission.CodeRequired);
            _code = value;
        }
    }

    public string Name
    {
        get => _name;
        set => _name = value ?? string.Empty;
    }

    public string Module
    {
        get => _module;
        set => _module = value ?? string.Empty;
    }

    public Permission(string code, string name, string module) : base()
    {
        Code = code;
        Name = name;
        Module = module;
    }

    public Permission() : base() { }
}
