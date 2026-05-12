using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechSalesManagement.Infrastructure.Persistence.Models;

namespace TechSalesManagement.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<RoleDbModel>
{
    public void Configure(EntityTypeBuilder<RoleDbModel> builder)
    {
        builder.ToTable("Role");
        builder.HasKey(x => x.id);
        builder.Property(x => x.name).IsRequired().HasMaxLength(100);
        builder.HasIndex(x => x.name).IsUnique();
        builder.Property(x => x.created_at).HasDefaultValueSql("now()");
    }
}
