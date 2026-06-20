using Ballcom.ProductCatalog.Application.DTOs;

namespace Ballcom.ProductCatalog.Application.Interfaces
{
    public interface IProductReadRepository
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(Guid id);
    }
}
