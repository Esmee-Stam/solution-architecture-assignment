using Ballcom.CustomerService.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.CustomerService.Infrastructure.Data
{
    public class CustomerDbContext(DbContextOptions<CustomerDbContext> options) : DbContext(options)
    {
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>()
                .HasIndex(c => c.PhoneNumber)
                .IsUnique();
        }
    }
}
