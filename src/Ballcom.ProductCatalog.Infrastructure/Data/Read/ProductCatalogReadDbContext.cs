using Ballcom.ProductCatalog.Domain.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Ballcom.ProductCatalog.Infrastructure.Data.Read
{
    public class ProductCatalogReadDbContext(DbContextOptions<ProductCatalogReadDbContext> options) : DbContext(options)
    {
        public DbSet<ProductReadModel> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProductReadModel>()
                .Property(entity => entity.PriceAmount)
                .HasPrecision(18, 2);

        }
    }
}
