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

            var existingOrder = await db.Orders
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == message.OrderId);

            if (existingOrder is null)
            {
                var newReadDto = new OrderDto
                {
                    Id = message.OrderId,
                    CustomerId = message.CustomerId,
                    Status = message.Status,
                    TotalAmount = message.Amount,
                    PaymentMethod = message.PaymentMethod,
                    CreatedAt = message.OccurredAt,
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

                await db.Orders.AddAsync(newReadDto);
            }
            else
            {
                existingOrder.Status = message.Status; 
                existingOrder.TotalAmount = message.Amount;

                existingOrder.OrderItems = message.Items.Select(i => new OrderItemDto
                {
                    Id = Guid.NewGuid(),
                    OrderId = message.OrderId,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.UnitPrice * i.Quantity
                }).ToList();
            }

            await db.SaveChangesAsync();
        }
    }
}

