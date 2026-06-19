using Ballcom.ProductCatalog.Application.DTOs;
using Ballcom.ProductCatalog.Infrastructure.Data.Read;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.ProductCatalog.Application.Queries.GetProductById
{
    public class GetProductByIdHandler(ProductCatalogReadDbContext context)
    {
        public async Task<ProductDto?> Handle(GetProductById query)
        {
            return await context.Products
                .Where(p => p.Id == query.Id)
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
