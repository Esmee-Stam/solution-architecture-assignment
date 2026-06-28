using Ballcom.Shipment.Application.DTOs;
using Ballcom.Shipment.Application.Interfaces;
using ShipmentEntity = Ballcom.Shipment.Domain.Shipments.Shipment;
using Events.ShipmentEvents;
using MassTransit;

namespace Ballcom.Shipment.Application.Commands.CreateShipmentFromPackedWarehouseOrder;

public class CreateShipmentFromPackedWarehouseOrderHandler(
    IShipmentWriteRepository writeRepository,
    IShipmentReadRepository readRepository,
    IPublishEndpoint publishEndpoint)
{
    public async Task<CreateShipmentFromPackedWarehouseOrderResult> Handle(
        CreateShipmentFromPackedWarehouseOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var existingShipment = await writeRepository.GetByWarehouseOrderIdAsync(command.WarehouseOrderId, cancellationToken)
            ?? await writeRepository.GetByOrderIdAsync(command.OrderId, cancellationToken);

        if (existingShipment is not null)
        {
            return new CreateShipmentFromPackedWarehouseOrderResult(existingShipment.Id, existingShipment.TrackingNumber);
        }

        var shipment = new ShipmentEntity(
            Guid.NewGuid(),
            command.WarehouseOrderId,
            command.OrderId,
            command.CustomerId,
            command.Carrier);

        await writeRepository.AddAsync(shipment, cancellationToken);
        await writeRepository.SaveChangesAsync(cancellationToken);

        await readRepository.UpsertAsync(ToDto(shipment), cancellationToken);

        await publishEndpoint.Publish(new ShipmentCreatedEvent(
            shipment.Id,
            shipment.WarehouseOrderId,
            shipment.OrderId,
            shipment.CustomerId,
            shipment.Status.ToString(),
            shipment.Carrier,
            shipment.TrackingNumber,
            DateTime.UtcNow
        ), cancellationToken);

        return new CreateShipmentFromPackedWarehouseOrderResult(shipment.Id, shipment.TrackingNumber);
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
