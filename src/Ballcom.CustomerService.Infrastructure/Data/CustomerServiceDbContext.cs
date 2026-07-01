using Ballcom.CustomerService.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.CustomerService.Infrastructure.Data;

public class CustomerServiceDbContext(DbContextOptions<CustomerServiceDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerOrder> CustomerOrders { get; set; }
    public DbSet<CustomerOrderItem> CustomerOrderItems { get; set; }
    public DbSet<CustomerShipment> CustomerShipments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(c => c.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(c => c.IdentityUserId)
                .HasMaxLength(450);

            entity.HasIndex(c => c.PhoneNumber)
                .IsUnique()
                .HasFilter("[PhoneNumber] IS NOT NULL");

            entity.HasIndex(c => c.IdentityUserId)
                .IsUnique()
                .HasFilter("[IdentityUserId] IS NOT NULL");
        });

        builder.Entity<CustomerOrder>(entity =>
        {
            entity.HasKey(order => order.OrderId);

            entity.Property(order => order.Status)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(order => order.TotalAmount)
                .HasPrecision(18, 2);

            entity.Property(order => order.Currency)
                .HasMaxLength(10)
                .IsRequired();

            entity.Property(order => order.PaymentMethod)
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(order => order.CustomerId);

            entity.HasMany(order => order.Items)
                .WithOne()
                .HasForeignKey(item => item.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<CustomerOrderItem>(entity =>
        {
            entity.HasKey(item => item.Id);

            entity.Property(item => item.ProductName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(item => item.UnitPrice)
                .HasPrecision(18, 2);

            entity.Property(item => item.Currency)
                .HasMaxLength(10)
                .IsRequired();

            entity.HasIndex(item => item.OrderId);
        });

        builder.Entity<CustomerShipment>(entity =>
        {
            entity.HasKey(shipment => shipment.ShipmentId);

            entity.Property(shipment => shipment.Status)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(shipment => shipment.Carrier)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(shipment => shipment.TrackingNumber)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(shipment => shipment.CustomerId);
            entity.HasIndex(shipment => shipment.OrderId);
            entity.HasIndex(shipment => shipment.TrackingNumber);
        });
    }
}
