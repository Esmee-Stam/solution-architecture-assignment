using Events.OrderEvents.ShoppingCartEvents;
using MassTransit;

namespace Ballcom.Order.Infrastructure.Messaging
{
    public class CartItemAddedConsumer : IConsumer<CartItemAddedEvent>
    {
        public Task Consume(ConsumeContext<CartItemAddedEvent> context)
        {
            var message = context.Message;

            Console.WriteLine($"CartItemAdded: {message.ProductId}, Quantity: {message.Quantity}");
            return Task.CompletedTask;
        }
    }
}
