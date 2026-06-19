using Ballcom.ProductCatalog.Domain.Domain;

namespace Ballcom.ProductCatalog.DomainServices.IRepository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(Guid id);
    }
}
