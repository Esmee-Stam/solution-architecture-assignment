using Ballcom.CustomerService.Application;
using Ballcom.CustomerService.Domain.Domain;
using Events.ShipmentEvents;
using MassTransit;

namespace Ballcom.CustomerService.Infrastructure.Messaging;

public class ShipmentDispatchedConsumer(ICustomerRepository repository) : IConsumer<ShipmentDispatchedEvent>
{
    public async Task Consume(ConsumeContext<ShipmentDispatchedEvent> context)
    {
        var message = context.Message;

        var shipment = new CustomerShipment
        {
            ShipmentId = message.ShipmentId,
            OrderId = message.OrderId,
            CustomerId = message.CustomerId,
            Status = message.Status,
            Carrier = message.Carrier,
            TrackingNumber = message.TrackingNumber,
            DispatchedAt = message.OccurredAt,
            LastUpdatedAt = message.OccurredAt
        };

        await repository.UpsertShipmentAsync(shipment, context.CancellationToken);
        await repository.SaveChangesAsync(context.CancellationToken);
    }
}
