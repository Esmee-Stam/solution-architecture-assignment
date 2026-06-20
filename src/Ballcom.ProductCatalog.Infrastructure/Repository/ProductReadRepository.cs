using Ballcom.ProductCatalog.Application.DTOs;
using Ballcom.ProductCatalog.Application.Interfaces;
using Ballcom.ProductCatalog.Domain.Domain;
using Ballcom.ProductCatalog.Infrastructure.Data.Read;
using Microsoft.EntityFrameworkCore;
namespace Ballcom.ProductCatalog.Infrastructure.Repository
{
    public class ProductReadRepository(ProductCatalogReadDbContext readContext) : IProductReadRepository
    {
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            return await readContext.Products
                    .Select(p => new ProductDto
                    (
                        p.Id,
                        p.Name,
                        p.Description,
                        p.PriceAmount,
                        p.Currency,
                        p.Stock
                    ))
                    .ToListAsync();
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            return await readContext.Products
                    .Where(p => p.Id == id)
                    .Select(p => new ProductDto(
                        p.Id,
                    p.Name,
                    p.Description,
                    p.PriceAmount,
                    p.Currency,
                    p.Stock
                    ))
                    .FirstOrDefaultAsync();
        }
    }
}