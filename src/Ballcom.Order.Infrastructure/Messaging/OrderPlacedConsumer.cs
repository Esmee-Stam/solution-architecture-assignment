using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Infrastructure.Data.Read;
using Events.OrderEvents;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Order.Infrastructure.Messaging
{
    public class OrderPlacedConsumer(OrderReadDbContext db) : IConsumer<OrderPlacedEvent>
    {
        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var message = context.Message;

            var exists = await db.Orders.AnyAsync(x => x.Id == message.OrderId);
            if (exists) return;

            var order = new OrderDto
            {
                Id = message.OrderId,
                CustomerId = message.CustomerId,
                PaymentMethod = message.PaymentMethod,
                Status = "Placed",
                TotalAmount = message.TotalAmount,
                CreatedAt = DateTime.UtcNow,
                OrderItems = message.Items.Select(i => new OrderItemDto
                {
                    Id = Guid.NewGuid(),
                    OrderId = message.OrderId,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.UnitPrice * i.Quantity
                }).ToList()
            };

            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();
        }
    }
}
