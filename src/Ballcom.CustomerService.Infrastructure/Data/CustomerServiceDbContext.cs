using Ballcom.CustomerService.Domain.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Ballcom.CustomerService.Infrastructure.Data
{
    public class CustomerServiceDbContext(DbContextOptions<CustomerServiceDbContext> options) : DbContext(options)
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
