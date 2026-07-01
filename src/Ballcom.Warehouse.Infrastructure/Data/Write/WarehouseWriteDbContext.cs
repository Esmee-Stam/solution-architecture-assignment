using Ballcom.Warehouse.Domain.WarehouseOrders;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Warehouse.Infrastructure.Data.Write;

public class WarehouseWriteDbContext(DbContextOptions<WarehouseWriteDbContext> options) : DbContext(options)
{
    public DbSet<WarehouseOrder> WarehouseOrders => Set<WarehouseOrder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WarehouseOrder>(entity =>
        {
            entity.ToTable("WarehouseOrders");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.OrderId).IsRequired();
            entity.Property(x => x.CustomerId).IsRequired();
            entity.Property(x => x.Status).HasConversion<int>().IsRequired();
            entity.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.Currency).HasMaxLength(10).IsRequired();
            entity.Property(x => x.PaymentMethod).HasMaxLength(50).IsRequired();
            entity.Property(x => x.CreatedAt).IsRequired();
            entity.Property(x => x.PickedAt);
            entity.Property(x => x.PackedAt);

            entity.HasIndex(x => x.OrderId).IsUnique();

            entity.OwnsMany(x => x.Items, items =>
            {
                items.ToTable("WarehouseOrderItems");
                items.WithOwner().HasForeignKey("WarehouseOrderId");
                items.HasKey(x => x.Id);

                items.Property(x => x.ProductId).IsRequired();
                items.Property(x => x.ProductName).HasMaxLength(200).IsRequired();
                items.Property(x => x.Quantity).IsRequired();
                items.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
                items.Property(x => x.Currency).HasMaxLength(10).IsRequired();
            }).UsePropertyAccessMode(PropertyAccessMode.Field);
        });
    }
}
