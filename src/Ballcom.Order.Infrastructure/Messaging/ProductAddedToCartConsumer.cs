using Events.ProductCatalogEvents;
using MassTransit;

namespace Ballcom.Order.Infrastructure.Messaging
{
    public class ProductAddedToCartConsumer : IConsumer<ProductAddedToCartEvent>
    {
        public async Task Consume(ConsumeContext<ProductAddedToCartEvent> context)
        {
            var message = context.Message;

            Console.WriteLine($"Received ProductAddedToCartEvent: CustomerId={message.CustomerId}, ProductId={message.ProductId}, ProductName={message.ProductName}, Price={message.Price}");
        }
    }
}
