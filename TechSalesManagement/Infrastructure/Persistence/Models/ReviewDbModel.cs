using System;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Infrastructure.Persistence.Models;

public class ReviewDbModel
{
    public Guid id { get; set; }
    public Guid? user_id { get; set; }
    public Guid product_id { get; set; }
    public int rating { get; set; }
    public string? comment { get; set; }
    public ReviewStatus? status { get; set; }
    public DateTimeOffset? created_at { get; set; }
    public DateTimeOffset? updated_at { get; set; }
}
