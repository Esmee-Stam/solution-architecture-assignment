using Ballcom.Order.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Order.Infrastructure.Data.Read;

public class OrderReadDbContext(DbContextOptions<OrderReadDbContext> options) : DbContext(options)
{
    public DbSet<OrderDto> Orders => Set<OrderDto>();
    public DbSet<OrderItemDto> OrderItems => Set<OrderItemDto>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<OrderDto>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();

            entity.Property(x => x.TotalAmount)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.HasMany(x => x.OrderItems)
                  .WithOne()
                  .HasForeignKey(x => x.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<OrderItemDto>(entity =>
        {
            entity.ToTable("OrderItems");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();

            entity.Property(x => x.UnitPrice)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(x => x.TotalPrice)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(x => x.ProductName)
                  .HasMaxLength(200)
                  .IsRequired();
        });
    }
}

