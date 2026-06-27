using Ballcom.Order.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Order.Infrastructure.Data;

public class ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options) : DbContext(options)
{
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ShoppingCart>(entity =>
        {
            entity.ToTable("ShoppingCarts");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();

            entity.Property(x => x.CustomerId).IsRequired();
            entity.Property(x => x.CreatedAt).IsRequired();

            entity.Property(x => x.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();

            entity.HasMany(x => x.CartItems)
                .WithOne()
                .HasForeignKey("ShoppingCartId")
                .OnDelete(DeleteBehavior.Cascade);

            // Setup the Backing Field Mapping
            entity.Navigation(x => x.CartItems)
                .HasField("_cartItems")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Entity<CartItem>(item =>
        {
            item.ToTable("CartItems");
            item.HasKey(x => x.Id);
            item.Property(x => x.Id).ValueGeneratedNever();

            item.Property(x => x.ProductId).IsRequired();
            item.Property(x => x.ProductName).HasMaxLength(200).IsRequired();
            item.Property(x => x.Quantity).IsRequired();

            item.OwnsOne(x => x.Price, p =>
            {
                p.Property(x => x.Amount).HasColumnName("PriceAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

                p.Property(x => x.Currency).HasColumnName("PriceCurrency").HasMaxLength(3).IsRequired();
            });
        });
    }
}
