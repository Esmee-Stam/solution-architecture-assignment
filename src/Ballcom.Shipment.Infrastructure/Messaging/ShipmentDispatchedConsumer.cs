using Ballcom.Shipment.Application.Interfaces;
using Events.ShipmentEvents;
using MassTransit;

namespace Ballcom.Shipment.Infrastructure.Messaging;

public class ShipmentDispatchedConsumer(IShipmentReadRepository readRepository) : IConsumer<ShipmentDispatchedEvent>
{
    public async Task Consume(ConsumeContext<ShipmentDispatchedEvent> context)
    {
        var message = context.Message;
        var shipment = await readRepository.GetByIdAsync(message.ShipmentId, context.CancellationToken);
        if (shipment is null) return;

        shipment.Status = message.Status;
        shipment.Carrier = message.Carrier;
        shipment.TrackingNumber = message.TrackingNumber;
        shipment.DispatchedAt = message.OccurredAt;

        await readRepository.UpsertAsync(shipment, context.CancellationToken);
    }
}
