using Ballcom.ProductCatalog.Application.Interfaces;
using Ballcom.ProductCatalog.Domain.Domain;
using Ballcom.ProductCatalog.Infrastructure.Data.Write;

namespace Ballcom.ProductCatalog.Infrastructure.Repository
{
    public class ProductWriteRepository(ProductCatalogWriteDbContext writeDbContext) : IProductWriteRepository
    {
        public async Task AddProductAsync(Product product)
        {
            await writeDbContext.Products.AddAsync(product);
            await writeDbContext.SaveChangesAsync();            
        }
    }
}
