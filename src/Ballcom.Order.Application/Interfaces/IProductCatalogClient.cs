using Ballcom.Order.Application.DTOs;

namespace Ballcom.Order.Application.Interfaces;

public interface IProductCatalogClient
{
    Task<ProductDto?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default);
}
