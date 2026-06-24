using Microsoft.EntityFrameworkCore;

namespace Ballcom.Payment.Infrastructure.Data.EventStore;

public class PaymentEventStoreDbContext(DbContextOptions<PaymentEventStoreDbContext> options)
    : DbContext(options)
{
    public DbSet<PaymentEventEntity> PaymentEvents => Set<PaymentEventEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentEventEntity>(entity =>
        {
            entity.ToTable("PaymentEvents");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.PaymentId)
                .IsRequired();

            entity.Property(e => e.EventType)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.EventDataJson)
                .IsRequired();

            entity.Property(e => e.OccurredAt)
                .IsRequired();

            entity.Property(e => e.Version)
                .IsRequired();

            entity.HasIndex(e => e.PaymentId);
        });
    }
}