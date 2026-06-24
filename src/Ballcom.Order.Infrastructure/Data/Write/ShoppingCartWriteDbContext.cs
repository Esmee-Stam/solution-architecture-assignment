using Ballcom.Order.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Order.Infrastructure.Data.Write;

public class ShoppingCartWriteDbContext(DbContextOptions<ShoppingCartWriteDbContext> options) : DbContext(options)
{
    public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.CustomerId);
            entity.Property(x => x.CreatedAt);

            entity.OwnsMany(x => x.CartItems, items =>
            {
                items.WithOwner().HasForeignKey("ShoppingCartId");
                items.Property<Guid>("Id");
                items.HasKey("Id");
                items.Property(x => x.ProductId).IsRequired();
                items.Property(x => x.ProductName).IsRequired().HasMaxLength(200);
                items.Property(x => x.Quantity).IsRequired();
            });
        });
    }
}
