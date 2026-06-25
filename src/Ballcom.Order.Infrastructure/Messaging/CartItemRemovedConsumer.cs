using MassTransit;

namespace Ballcom.Order.Infrastructure.Messaging
{
    public class CartItemRemovedConsumer : IConsumer<CartItemRemovedConsumer>
    {
        public Task Consume(ConsumeContext<CartItemRemovedConsumer> context)
        {
            var message = context.Message;

            Console.WriteLine(message);

            return Task.CompletedTask;
        }
    }
}
