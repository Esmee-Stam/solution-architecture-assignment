using OrderAggregate = Ballcom.Order.Domain.Domain.Order;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Order.Infrastructure.Data.Write;

public class OrderWriteDbContext(DbContextOptions<OrderWriteDbContext> options) : DbContext(options)
{
    public DbSet<OrderAggregate> Orders => Set<OrderAggregate>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<OrderAggregate>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.CustomerId).IsRequired();
            entity.Property(x => x.PaymentMethod).HasConversion<int>().IsRequired();
            entity.Property(x => x.Status).HasConversion<int>().IsRequired();
            entity.Property(x => x.CreatedAt).IsRequired();

            entity.Property(x => x.RowVersion).IsRowVersion();

            entity.OwnsMany(x => x.OrderItems, items =>
            {
                items.ToTable("OrderItems");

                items.WithOwner().HasForeignKey("OrderId");
                items.HasKey("Id");

                items.Property(x => x.ProductId).IsRequired();
                items.Property(x => x.ProductName).IsRequired().HasMaxLength(200);
                items.Property(x => x.Quantity).IsRequired();

                items.OwnsOne(x => x.UnitPrice, price =>
                {
                    price.Property(p => p.Amount).HasColumnName("PriceAmount")
                        .HasColumnType("decimal(18,2)")
                        .IsRequired();
                    price.Property(p => p.Currency).HasColumnName("PriceCurrency")
                        .HasColumnType("nvarchar(3)")
                        .IsRequired();
                });
            }).UsePropertyAccessMode(PropertyAccessMode.Field);
        });
    }
}