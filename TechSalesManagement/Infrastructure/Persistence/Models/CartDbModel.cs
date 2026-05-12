using System;

namespace TechSalesManagement.Infrastructure.Persistence.Models;

public class CartDbModel
{
    public Guid id { get; set; }
    public Guid user_id { get; set; }
    public DateTimeOffset created_at { get; set; }
}
