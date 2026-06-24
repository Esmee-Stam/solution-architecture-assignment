namespace Events.ProductCatalogEvents
{
    public record ProductAddedToCartEvent(
        string CustomerId,
        Guid ProductId,
        string ProductName,
        decimal Price,
        string Currency,
        int Quantity
    );
}
