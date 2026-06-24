using System.Net.Http.Json;
using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Interfaces;

namespace Ballcom.Order.Infrastructure.Integration;

public class ProductCatalogHttpClient(HttpClient httpClient) : IProductCatalogClient
{
    public async Task<ProductDto?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<ProductDto>($"/api/products/{productId}", cancellationToken);
    }
}
