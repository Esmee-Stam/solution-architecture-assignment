using Ballcom.ProductCatalog.Application.DTOs;
using Ballcom.ProductCatalog.Application.Interfaces;

namespace Ballcom.ProductCatalog.Application.Queries.GetAllProducts
{
    public class GetAllProductsHandler(IProductReadRepository readRepository)
    {
        public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery query)
        {
            return await readRepository.GetAllProductsAsync();
        }
    }
}
