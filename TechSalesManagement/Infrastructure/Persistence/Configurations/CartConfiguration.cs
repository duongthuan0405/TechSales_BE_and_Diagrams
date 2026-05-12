using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechSalesManagement.Infrastructure.Persistence.Models;

namespace TechSalesManagement.Infrastructure.Persistence.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<CartDbModel>
{
    public void Configure(EntityTypeBuilder<CartDbModel> builder)
    {
        builder.ToTable("Cart");
        builder.HasKey(x => x.id);
        builder.HasIndex(x => x.user_id).IsUnique();
        builder.Property(x => x.created_at).HasDefaultValueSql("now()");
    }
}
