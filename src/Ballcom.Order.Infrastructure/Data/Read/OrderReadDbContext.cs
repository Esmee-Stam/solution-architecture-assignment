using Ballcom.Order.Infrastructure.Data.Read.Models;
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
            entity.HasKey(x => x.Id);
            entity.HasMany(x => x.OrderItems).WithOne().HasForeignKey(x => x.OrderId);
        });
    }
}
