namespace Events.OrderEvents.ShoppingCartEvents
{
    public record CartItemRemovedEvent(Guid CustomerId, Guid ProductId);
}
