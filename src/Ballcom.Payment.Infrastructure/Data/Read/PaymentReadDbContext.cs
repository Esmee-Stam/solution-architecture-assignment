using Microsoft.EntityFrameworkCore;

namespace Ballcom.Payment.Infrastructure.Data.Read;

public class PaymentReadDbContext(DbContextOptions<PaymentReadDbContext> options)
    : DbContext(options)
{
    public DbSet<PaymentReadModel> Payments => Set<PaymentReadModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentReadModel>(entity =>
        {
            entity.ToTable("PaymentReadModels");

            entity.HasKey(e => e.PaymentId);

            entity.Property(e => e.OrderId)
                .IsRequired();

            entity.Property(e => e.CustomerId)
                .IsRequired();

            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.Currency)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(e => e.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.FailureReason)
                .HasMaxLength(500);

            entity.HasIndex(e => e.OrderId);
        });
    }
}