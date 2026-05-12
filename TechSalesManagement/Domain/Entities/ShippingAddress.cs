using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class ShippingAddress : BaseEntity
{
    public Guid user_id { get; set; }
    public string province { get; set; } = string.Empty;
    public string ward { get; set; } = string.Empty;
    public string detail { get; set; } = string.Empty;
    public bool is_default { get; set; }
    public DateTimeOffset? deleted_at { get; set; }

    public ShippingAddress(Guid userId, string province, string ward, string detail)
    {
        user_id = userId;
        this.province = province;
        this.ward = ward;
        this.detail = detail;
    }

    public ShippingAddress() { }
}
