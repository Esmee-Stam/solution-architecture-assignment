using Ballcom.ProductCatalog.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.ProductCatalog.Infrastructure.Data
{
    public class ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            builder.Entity<Product>().HasData(new Product
            {
                Id = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                Name = "E-Reader",
                Description = "Duurzame e-reader inclusief stylus voor notities, 10,3 inch scherm met comfortLight.",
                Price = 399.95m,
                StockQuantity = 42
            });
        }
    }
}
