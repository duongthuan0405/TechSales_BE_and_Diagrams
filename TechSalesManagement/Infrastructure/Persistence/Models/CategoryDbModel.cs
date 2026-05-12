using System;

namespace TechSalesManagement.Infrastructure.Persistence.Models;

public class CategoryDbModel
{
    public Guid id { get; set; }
    public string name { get; set; } = string.Empty;
    public DateTimeOffset created_at { get; set; }
}
