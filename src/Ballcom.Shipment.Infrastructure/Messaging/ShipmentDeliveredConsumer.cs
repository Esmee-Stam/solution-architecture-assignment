using Ballcom.Shipment.Application.Interfaces;
using Events.ShipmentEvents;
using MassTransit;

namespace Ballcom.Shipment.Infrastructure.Messaging;

public class ShipmentDeliveredConsumer(IShipmentReadRepository readRepository) : IConsumer<ShipmentDeliveredEvent>
{
    public async Task Consume(ConsumeContext<ShipmentDeliveredEvent> context)
    {
        var message = context.Message;
        var shipment = await readRepository.GetByIdAsync(message.ShipmentId, context.CancellationToken);
        if (shipment is null) return;

        shipment.Status = message.Status;
        shipment.Carrier = message.Carrier;
        shipment.TrackingNumber = message.TrackingNumber;
        shipment.DeliveredAt = message.OccurredAt;

        await readRepository.UpsertAsync(shipment, context.CancellationToken);
    }
}
