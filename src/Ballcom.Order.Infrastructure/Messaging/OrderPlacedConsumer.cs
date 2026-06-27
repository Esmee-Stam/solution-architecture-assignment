using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Interfaces;
using Events.OrderEvents;
using MassTransit;

namespace Ballcom.Order.Infrastructure.Messaging
{
    public class OrderPlacedConsumer(IOrderReadRepository repository) : IConsumer<OrderPlacedEvent>
    {
        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var message = context.Message;

            var exists = await repository.GetByIdAsync(message.OrderId);
            if (exists is not null) return;

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

            await repository.UpsertAsync(order);
        }
    }
}
