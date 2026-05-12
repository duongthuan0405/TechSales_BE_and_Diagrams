using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechSalesManagement.Infrastructure.Persistence.Models;

namespace TechSalesManagement.Infrastructure.Persistence.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermissionDbModel>
{
    public void Configure(EntityTypeBuilder<RolePermissionDbModel> builder)
    {
        builder.ToTable("RolePermission");
        builder.HasKey(x => new { x.role_id, x.permission_id });
    }
}
