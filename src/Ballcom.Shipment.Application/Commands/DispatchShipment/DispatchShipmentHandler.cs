using Ballcom.Shipment.Application.DTOs;
using Ballcom.Shipment.Application.Interfaces;
using ShipmentEntity = Ballcom.Shipment.Domain.Shipments.Shipment;
using Events.ShipmentEvents;
using MassTransit;

namespace Ballcom.Shipment.Application.Commands.DispatchShipment;

public class DispatchShipmentHandler(
    IShipmentWriteRepository writeRepository,
    IShipmentReadRepository readRepository,
    IPublishEndpoint publishEndpoint)
{
    public async Task Handle(DispatchShipmentCommand command, CancellationToken cancellationToken = default)
    {
        var shipment = await writeRepository.GetByIdAsync(command.ShipmentId, cancellationToken)
            ?? throw new InvalidOperationException("Shipment was not found.");

        shipment.Dispatch();

        await writeRepository.SaveChangesAsync(cancellationToken);
        await readRepository.UpsertAsync(ToDto(shipment), cancellationToken);

        await publishEndpoint.Publish(new ShipmentDispatchedEvent(
            shipment.Id,
            shipment.OrderId,
            shipment.CustomerId,
            shipment.Status.ToString(),
            shipment.Carrier,
            shipment.TrackingNumber,
            DateTime.UtcNow
        ), cancellationToken);
    }

    private static ShipmentDto ToDto(ShipmentEntity shipment)
    {
        return new ShipmentDto
        {
            Id = shipment.Id,
            WarehouseOrderId = shipment.WarehouseOrderId,
            OrderId = shipment.OrderId,
            CustomerId = shipment.CustomerId,
            Status = shipment.Status.ToString(),
            Carrier = shipment.Carrier,
            TrackingNumber = shipment.TrackingNumber,
            CreatedAt = shipment.CreatedAt,
            DispatchedAt = shipment.DispatchedAt,
            DeliveredAt = shipment.DeliveredAt
        };
    }
}
