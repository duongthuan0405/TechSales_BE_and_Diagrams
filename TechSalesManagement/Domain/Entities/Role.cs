using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class Role : BaseEntity
{
    public string name { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;

    public List<Permission> permissions { get; set; } = new();

    public Role(string name, string description)
    {
        this.name = name;
        this.description = description;
    }

    public Role() { }
}
