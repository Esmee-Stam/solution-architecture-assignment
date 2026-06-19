using Ballcom.ProductCatalog.Application.DTOs;
using Ballcom.ProductCatalog.Infrastructure.Data.Read;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.ProductCatalog.Application.Queries.GetAllProducts
{
    public class GetAllProductsHandler(ProductCatalogReadDbContext context)
    {
        public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery query)
        {
            return await context.Products
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
    }
}
