namespace Ballcom.ProductCatalog.WebAPI.Models
{
    public record AddToCartModel(
        Guid ProductId,
        int Quantity
    );
}