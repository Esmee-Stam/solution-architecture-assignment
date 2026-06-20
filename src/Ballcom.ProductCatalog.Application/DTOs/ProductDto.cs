namespace Ballcom.ProductCatalog.Application.DTOs
{
    public record ProductDto(
         Guid Id,
         string Name,
         string Description,
         decimal PriceAmount,
         string Currency,
         int Stock
    );
}
