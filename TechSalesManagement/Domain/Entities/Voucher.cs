using System;
using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Domain.Entities;

public class Voucher : BaseEntity
{
    public string code { get; set; } = string.Empty;
    public VoucherType type { get; set; }
    public decimal value { get; set; }
    public int max_usage { get; set; }
    public int used_count { get; set; }
    public decimal min_order_amount { get; set; }
    public DateTimeOffset? start_date { get; set; }
    public DateTimeOffset? end_date { get; set; }
    public bool is_active { get; set; }

    public Voucher(string code, VoucherType type, decimal value)
    {
        this.code = code;
        this.type = type;
        this.value = value;
        is_active = true;
    }

    public Voucher() { }
}
