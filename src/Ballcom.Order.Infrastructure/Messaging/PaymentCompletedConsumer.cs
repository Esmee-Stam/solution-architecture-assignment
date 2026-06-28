using Ballcom.Order.Application.Interfaces;
using Events.OrderEvents;
using Events.OrderEvents.Dto;
using Events.PaymentEvents;
using MassTransit;

namespace Ballcom.Order.Infrastructure.Messaging
{
    public class PaymentCompletedConsumer(IOrderWriteRepository orderWriteRepository, IPublishEndpoint endpoint) : IConsumer<PaymentCompletedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var message = context.Message;

            var order = await orderWriteRepository.GetOrderByIdAsync(message.OrderId);

            if (order is null) throw new Exception("Order not found");

            order.MarkAsPaid();

            await orderWriteRepository.SaveChangesAsync();

            await endpoint.Publish(new OrderStatusChangedEvent(
                    order.Id,
                    order.CustomerId,
                    order.Status.ToString(),
                    order.TotalPrice.Amount,
                    order.TotalPrice.Currency,
                    order.PaymentMethod.ToString(),
                    order.OrderItems.Select(i => new OrderItemEventDto(
                        i.Id,
                        i.ProductId,
                        i.ProductName,
                        i.Quantity,
                        i.UnitPrice.Amount,
                        i.UnitPrice.Currency
                    )).ToList(),
                    DateTime.UtcNow
            ));
        }
    }
}
