using System;

namespace TechSalesManagement.Infrastructure.Persistence.Models;

public class InventoryDbModel
{
    public Guid product_id { get; set; }
    public int quantity { get; set; }
    public int reserved_quantity { get; set; }
}
