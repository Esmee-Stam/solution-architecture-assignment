using Ballcom.Warehouse.Application.Commands.CreateWarehouseOrderFromPaidOrder;
using Events.OrderEvents;
using MassTransit;

namespace Ballcom.Warehouse.Infrastructure.Messaging;

public class PaidOrderConsumer(CreateWarehouseOrderFromPaidOrderHandler handler) : IConsumer<OrderStatusChangedEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedEvent> context)
    {
        var message = context.Message;

        if (!string.Equals(message.Status, "Paid", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        await handler.Handle(new CreateWarehouseOrderFromPaidOrderCommand(
            message.OrderId,
            message.CustomerId,
            message.PaymentMethod,
            message.Amount,
            message.Currency,
            message.Items
        ), context.CancellationToken);
    }
}
