namespace Events.ProductCatalogEvents
{
    public record ProductCreatedEvent(
        Guid Id,
        string Name,
        string Description,
        decimal PriceAmount,
        string Currency,
        int Stock
    );
}
