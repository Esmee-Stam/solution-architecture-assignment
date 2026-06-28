using Microsoft.EntityFrameworkCore;

namespace Ballcom.Shipment.Infrastructure.Data.Read;

public class ShipmentReadDbContext(DbContextOptions<ShipmentReadDbContext> options) : DbContext(options)
{
    public DbSet<ShipmentReadModel> Shipments => Set<ShipmentReadModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShipmentReadModel>(entity =>
        {
            entity.ToTable("ShipmentReadModels");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.OrderId).IsUnique();
            entity.HasIndex(x => x.WarehouseOrderId).IsUnique();
            entity.HasIndex(x => x.TrackingNumber).IsUnique();
            entity.Property(x => x.Status).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Carrier).HasMaxLength(100).IsRequired();
            entity.Property(x => x.TrackingNumber).HasMaxLength(100).IsRequired();
        });
    }
}
