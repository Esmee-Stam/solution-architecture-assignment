namespace Events.OrderEvents.ShoppingCartEvents
{
    public record CartItemAddedEvent(Guid CustomerId, Guid ProductId, string ProductName, int Quantity);
}
