using Microsoft.EntityFrameworkCore;

namespace Ballcom.Warehouse.Infrastructure.Data.Read;

public class WarehouseReadDbContext(DbContextOptions<WarehouseReadDbContext> options) : DbContext(options)
{
    public DbSet<WarehouseOrderReadModel> WarehouseOrders => Set<WarehouseOrderReadModel>();
    public DbSet<WarehouseOrderItemReadModel> WarehouseOrderItems => Set<WarehouseOrderItemReadModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WarehouseOrderReadModel>(entity =>
        {
            entity.ToTable("WarehouseOrderReadModels");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.OrderId).IsRequired();
            entity.Property(x => x.CustomerId).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(50).IsRequired();
            entity.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.Currency).HasMaxLength(10).IsRequired();
            entity.Property(x => x.PaymentMethod).HasMaxLength(50).IsRequired();
            entity.Property(x => x.CreatedAt).IsRequired();

            entity.HasIndex(x => x.OrderId).IsUnique();

            entity.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey(x => x.WarehouseOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WarehouseOrderItemReadModel>(entity =>
        {
            entity.ToTable("WarehouseOrderItemReadModels");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.ProductId).IsRequired();
            entity.Property(x => x.ProductName).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Quantity).IsRequired();
            entity.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.Currency).HasMaxLength(10).IsRequired();
        });
    }
}
