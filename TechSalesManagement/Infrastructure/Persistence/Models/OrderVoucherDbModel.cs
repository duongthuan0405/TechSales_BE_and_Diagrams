using System;

namespace TechSalesManagement.Infrastructure.Persistence.Models;

public class OrderVoucherDbModel
{
    public Guid order_id { get; set; }
    public Guid voucher_id { get; set; }
}
