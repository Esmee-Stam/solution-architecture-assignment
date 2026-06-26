using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Domain.ValueObjects;
using Events.OrderEvents;
using Events.PaymentEvents;
using MassTransit;

namespace Ballcom.Order.Infrastructure.Messaging
{
    public class PaymentCompletedConsumer(IOrderWriteRepository orderWriteRepository) : IConsumer<PaymentCompletedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var message = context.Message;

            var order = await orderWriteRepository.GetOrderByIdAsync(message.OrderId);

            if (order is null) throw new Exception("Order not found");

            order.Confirm();
            order.MarkAsPaid();

            await orderWriteRepository.SaveChangesAsync();

            await context.Publish(new OrderStatusChangedEvent(
                    order.Id,
                    order.CustomerId,
                    order.Status.ToString(),
                    order.TotalPrice.Amount,
                    order.TotalPrice.Currency,
                    order.PaymentMethod.ToString(),
                    order.OrderItems.Select(i => new OrderItemEventDto(
                        i.ProductId,
                        i.ProductName,
                        i.Quantity,
                        i.UnitPrice.Amount
                    )).ToList(),
                    DateTime.UtcNow
                ));
        }
    }
}
