using Ballcom.Order.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Order.Infrastructure.Data.Read;

public class OrderReadDbContext(DbContextOptions<OrderReadDbContext> options) : DbContext(options)
{
    public DbSet<OrderReadModel> Orders => Set<OrderReadModel>();
    public DbSet<OrderItemReadModel> OrderItems => Set<OrderItemReadModel>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<OrderReadModel>(entity =>
        {
            entity.ToTable("Orders");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedNever();

            entity.Property(x => x.TotalAmount)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(x => x.PaymentMethod)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(x => x.Status)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(x => x.Currency)
                  .HasMaxLength(3)
                  .IsRequired();

            entity.HasMany(x => x.OrderItems)
                  .WithOne(x => x.Order)
                  .HasForeignKey(x => x.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<OrderItemReadModel>(entity =>
        {
            entity.ToTable("OrderItems");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedNever();

            entity.Property(x => x.ProductName)
                  .HasMaxLength(200)
                  .IsRequired();

            entity.Property(x => x.UnitPrice)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(x => x.TotalPrice)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(x => x.Quantity).IsRequired();                    
        });
    }
}

