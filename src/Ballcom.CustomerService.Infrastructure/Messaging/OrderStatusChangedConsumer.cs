using Ballcom.CustomerService.Application;
using Ballcom.CustomerService.Domain.Domain;
using Events.OrderEvents;
using MassTransit;

namespace Ballcom.CustomerService.Infrastructure.Messaging;

public class OrderStatusChangedConsumer(ICustomerRepository repository) : IConsumer<OrderStatusChangedEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedEvent> context)
    {
        var message = context.Message;

        var order = new CustomerOrder
        {
            OrderId = message.OrderId,
            CustomerId = message.CustomerId,
            Status = message.Status,
            TotalAmount = message.Amount,
            Currency = message.Currency,
            PaymentMethod = message.PaymentMethod,
            LastStatusChangedAt = message.OccurredAt,
            // A status update should not rewrite the order lines in the CustomerService read model.
            // The order lines are created/updated by OrderPlacedConsumer. Keeping this empty avoids
            // EF concurrency issues when OrderPlaced and OrderStatusChanged are processed close together.
            Items = []
        };

        await repository.UpsertOrderAsync(order, context.CancellationToken);
        await repository.SaveChangesAsync(context.CancellationToken);
    }
}
