namespace Ballcom.ProductCatalog.Application.Commands.CreateProduct
{
    public record CreateProductCommand(
        string Name,
        string Description,
        decimal PriceAmount,
        string Currency,
        int Stock
    );
}
