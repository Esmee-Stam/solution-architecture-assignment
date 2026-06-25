using Ballcom.Order.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Order.Infrastructure.Data.Write;

public class ShoppingCartWriteDbContext(DbContextOptions<ShoppingCartWriteDbContext> options) : DbContext(options)
{
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.CustomerId)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.HasMany(x => x.CartItems)
                .WithOne()
                .HasForeignKey(x => x.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<CartItem>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.ProductId)
                .IsRequired();

            entity.Property(x => x.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Quantity)
                .IsRequired();

            entity.Property(x => x.ShoppingCartId)
                .IsRequired();
        });
    }
}
