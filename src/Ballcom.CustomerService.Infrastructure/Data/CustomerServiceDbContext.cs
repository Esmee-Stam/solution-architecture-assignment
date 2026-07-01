using Ballcom.CustomerService.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.CustomerService.Infrastructure.Data;

public class CustomerServiceDbContext(DbContextOptions<CustomerServiceDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }

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

            entity.HasIndex(c => c.PhoneNumber)
                .IsUnique()
                .HasFilter("[PhoneNumber] IS NOT NULL");
        });
    }
}
