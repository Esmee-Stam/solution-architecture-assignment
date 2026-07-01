using Ballcom.CustomerService.Application;
using Ballcom.CustomerService.Domain.Domain;
using Events.ShipmentEvents;
using MassTransit;

namespace Ballcom.CustomerService.Infrastructure.Messaging;

public class ShipmentCreatedConsumer(ICustomerRepository repository) : IConsumer<ShipmentCreatedEvent>
{
    public async Task Consume(ConsumeContext<ShipmentCreatedEvent> context)
    {
        var message = context.Message;

        var shipment = new CustomerShipment
        {
            ShipmentId = message.ShipmentId,
            WarehouseOrderId = message.WarehouseOrderId,
            OrderId = message.OrderId,
            CustomerId = message.CustomerId,
            Status = message.Status,
            Carrier = message.Carrier,
            TrackingNumber = message.TrackingNumber,
            CreatedAt = message.OccurredAt,
            LastUpdatedAt = message.OccurredAt
        };

        await repository.UpsertShipmentAsync(shipment, context.CancellationToken);
        await repository.SaveChangesAsync(context.CancellationToken);
    }
}
