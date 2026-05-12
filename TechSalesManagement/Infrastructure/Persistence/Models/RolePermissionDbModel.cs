using System;

namespace TechSalesManagement.Infrastructure.Persistence.Models;

public class RolePermissionDbModel
{
    public Guid role_id { get; set; }
    public Guid permission_id { get; set; }
}
