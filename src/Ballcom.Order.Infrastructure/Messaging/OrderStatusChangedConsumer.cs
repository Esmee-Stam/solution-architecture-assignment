using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Infrastructure.Data.Read;
using Events.OrderEvents;
using MassTransit;

namespace Ballcom.Order.Infrastructure.Messaging
{
    public class OrderStatusChangedConsumer(IOrderReadRepository repository) : IConsumer<OrderStatusChangedEvent>
    {

        public async Task Consume(ConsumeContext<OrderStatusChangedEvent> context)
        {
            var message = context.Message;

            var order = await repository.GetByIdAsync(message.OrderId);

            if (order == null)
            {
                Console.WriteLine($"Order not in read model yet: {message.OrderId}");
                return;
            }

            order.Status = message.Status;

            await repository.UpsertAsync(order);
        }
    }
}

