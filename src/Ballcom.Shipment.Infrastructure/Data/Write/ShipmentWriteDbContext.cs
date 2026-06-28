using ShipmentEntity = Ballcom.Shipment.Domain.Shipments.Shipment;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Shipment.Infrastructure.Data.Write;

public class ShipmentWriteDbContext(DbContextOptions<ShipmentWriteDbContext> options) : DbContext(options)
{
    public DbSet<ShipmentEntity> Shipments => Set<ShipmentEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShipmentEntity>(entity =>
        {
            entity.ToTable("Shipments");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.OrderId).IsUnique();
            entity.HasIndex(x => x.WarehouseOrderId).IsUnique();
            entity.HasIndex(x => x.TrackingNumber).IsUnique();
            entity.Property(x => x.Status).HasConversion<int>();
            entity.Property(x => x.Carrier).HasMaxLength(100).IsRequired();
            entity.Property(x => x.TrackingNumber).HasMaxLength(100).IsRequired();
        });
    }
}
