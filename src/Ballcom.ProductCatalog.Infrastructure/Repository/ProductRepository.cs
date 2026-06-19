using Ballcom.ProductCatalog.Domain.Domain;
using Ballcom.ProductCatalog.DomainServices.IRepository;
using Ballcom.ProductCatalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.ProductCatalog.Infrastructure.Repository
{
    public class ProductRepository(ProductCatalogDbContext context) : IProductRepository
    {     
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
           return await context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
