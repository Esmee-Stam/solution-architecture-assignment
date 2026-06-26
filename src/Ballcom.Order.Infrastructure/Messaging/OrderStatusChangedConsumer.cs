using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Infrastructure.Data.Read;
using Events.OrderEvents;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Order.Infrastructure.Messaging
{
    public class OrderStatusChangedConsumer(OrderReadDbContext db) : IConsumer<OrderStatusChangedEvent>
    {

        public async Task Consume(ConsumeContext<OrderStatusChangedEvent> context)
        {
            var message = context.Message;

            var order = await db.Orders
                .FirstOrDefaultAsync(x => x.Id == message.OrderId);

            if (order == null)
            {
                Console.WriteLine($"Order not in read model yet: {message.OrderId}");
                return;
            }

            order.Status = message.Status;

            await db.SaveChangesAsync();
        }
    }
}

