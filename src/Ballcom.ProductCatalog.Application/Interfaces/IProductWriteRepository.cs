using Ballcom.ProductCatalog.Domain.Domain;

namespace Ballcom.ProductCatalog.Application.Interfaces
{
    public interface IProductWriteRepository
    {
        Task AddProductAsync(Product product);
    }
}
