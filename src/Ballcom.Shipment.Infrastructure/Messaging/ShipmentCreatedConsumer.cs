using Ballcom.Shipment.Application.DTOs;
using Ballcom.Shipment.Application.Interfaces;
using Events.ShipmentEvents;
using MassTransit;

namespace Ballcom.Shipment.Infrastructure.Messaging;

public class ShipmentCreatedConsumer(IShipmentReadRepository readRepository) : IConsumer<ShipmentCreatedEvent>
{
    public async Task Consume(ConsumeContext<ShipmentCreatedEvent> context)
    {
        var message = context.Message;

        await readRepository.UpsertAsync(new ShipmentDto
        {
            Id = message.ShipmentId,
            WarehouseOrderId = message.WarehouseOrderId,
            OrderId = message.OrderId,
            CustomerId = message.CustomerId,
            Status = message.Status,
            Carrier = message.Carrier,
            TrackingNumber = message.TrackingNumber,
            CreatedAt = message.OccurredAt
        }, context.CancellationToken);
    }
}
