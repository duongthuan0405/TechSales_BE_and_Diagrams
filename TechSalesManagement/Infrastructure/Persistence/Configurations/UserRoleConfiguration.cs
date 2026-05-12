using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechSalesManagement.Infrastructure.Persistence.Models;

namespace TechSalesManagement.Infrastructure.Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoleDbModel>
{
    public void Configure(EntityTypeBuilder<UserRoleDbModel> builder)
    {
        builder.ToTable("UserRole");
        builder.HasKey(x => new { x.user_id, x.role_id });
    }
}
