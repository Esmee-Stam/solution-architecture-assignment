using Ballcom.Order.Infrastructure.Data.Read.Models;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Order.Infrastructure.Data.Read;

public class ShoppingCartReadDbContext(DbContextOptions<ShoppingCartReadDbContext> options) : DbContext(options)
{
    public DbSet<ShoppingCartReadModel> ShoppingCarts => Set<ShoppingCartReadModel>();
    public DbSet<CartItemReadModel> CartItems => Set<CartItemReadModel>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ShoppingCartReadModel>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasMany(x => x.CartItems).WithOne().HasForeignKey(x => x.ShoppingCartId);
        });
    }
}
