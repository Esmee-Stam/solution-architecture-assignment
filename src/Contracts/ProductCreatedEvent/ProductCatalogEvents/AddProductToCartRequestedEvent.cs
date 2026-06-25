namespace Events.ProductCatalogEvents
{
    public record AddProductToCartRequestedEvent(
        Guid CustomerId,
        Guid ProductId,
        string ProductName,
        decimal Price,
        string Currency,
        int Quantity
    );
}
