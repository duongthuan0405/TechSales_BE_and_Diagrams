using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechSalesManagement.Infrastructure.Persistence.Models;

namespace TechSalesManagement.Infrastructure.Persistence.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItemDbModel>
{
    public void Configure(EntityTypeBuilder<CartItemDbModel> builder)
    {
        builder.ToTable("CartItem");
        
        builder.HasKey(x => new { x.cart_id, x.product_id });
        
        builder.Property(x => x.created_at).HasDefaultValueSql("now()");
        builder.Property(x => x.updated_at).HasDefaultValueSql("now()");
    }
}
