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
            entity.HasKey(x => x.Id);
            entity.Property(x => x.CustomerId);
            entity.Property(x => x.CreatedAt);
            entity.Property(x => x.PaymentMethod).HasConversion<int>();
            entity.Property(x => x.Status).HasConversion<int>();

            entity.OwnsMany(x => x.OrderItems, items =>
            {
                items.WithOwner().HasForeignKey("OrderId");
                items.Property<Guid>("Id");
                items.HasKey("Id");
                items.Property(x => x.ProductId).IsRequired();
                items.Property(x => x.ProductName).IsRequired().HasMaxLength(200);
                items.Property(x => x.Quantity).IsRequired();
            });
        });
    }
}