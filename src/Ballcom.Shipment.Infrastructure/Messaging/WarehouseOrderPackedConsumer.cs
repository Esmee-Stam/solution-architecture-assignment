using Ballcom.Shipment.Application.Commands.CreateShipmentFromPackedWarehouseOrder;
using Events.WarehouseEvents;
using MassTransit;

namespace Ballcom.Shipment.Infrastructure.Messaging;

public class WarehouseOrderPackedConsumer(CreateShipmentFromPackedWarehouseOrderHandler handler) : IConsumer<WarehouseOrderPackedEvent>
{
    public async Task Consume(ConsumeContext<WarehouseOrderPackedEvent> context)
    {
        var message = context.Message;

        if (!string.Equals(message.Status, "Packed", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        await handler.Handle(new CreateShipmentFromPackedWarehouseOrderCommand(
            message.WarehouseOrderId,
            message.OrderId,
            message.CustomerId,
            "PostNL"
        ), context.CancellationToken);
    }
}
