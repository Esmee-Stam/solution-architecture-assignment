using Ballcom.ProductCatalog.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.ProductCatalog.Infrastructure.Data.Write
{
    public class ProductCatalogWriteDbContext(DbContextOptions<ProductCatalogWriteDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>(entity =>
            {
                entity.HasKey(entity => entity.Id);

                entity.Property(entity => entity.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(entity => entity.Description).IsRequired();

                entity.OwnsOne(entity => entity.Price, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("PriceAmount").HasPrecision(18,2);
                    money.Property(m => m.Currency).HasColumnName("PriceCurrency").HasMaxLength(3);
                });

                entity.OwnsOne(entity => entity.Quantity, stock =>
                {
                    stock.Property(s => s.Value).HasColumnName("Stock");
                });
            });
        }
    }
}
