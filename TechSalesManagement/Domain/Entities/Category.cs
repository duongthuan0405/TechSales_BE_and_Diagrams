using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class Category : BaseEntity
{
    public string name { get; set; } = string.Empty;

    public Category(string name)
    {
        this.name = name;
    }

    public Category() { }
}
