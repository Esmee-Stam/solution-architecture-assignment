using Ballcom.ProductCatalog.Application.DTOs;
using Ballcom.ProductCatalog.Application.Interfaces;

namespace Ballcom.ProductCatalog.Application.Queries.GetProductById
{
    public class GetProductByIdHandler(IProductReadRepository readRepository)
    {
        public async Task<ProductDto?> Handle(GetProductById query)
        {
            return await readRepository.GetProductByIdAsync(query.Id);
        }
    }
}
