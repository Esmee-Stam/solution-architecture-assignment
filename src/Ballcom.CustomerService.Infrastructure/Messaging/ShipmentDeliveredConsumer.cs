using Ballcom.CustomerService.Application;
using Ballcom.CustomerService.Domain.Domain;
using Events.ShipmentEvents;
using MassTransit;

namespace Ballcom.CustomerService.Infrastructure.Messaging;

public class ShipmentDeliveredConsumer(ICustomerRepository repository) : IConsumer<ShipmentDeliveredEvent>
{
    public async Task Consume(ConsumeContext<ShipmentDeliveredEvent> context)
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
            DeliveredAt = message.OccurredAt,
            LastUpdatedAt = message.OccurredAt
        };

        await repository.UpsertShipmentAsync(shipment, context.CancellationToken);
        await repository.SaveChangesAsync(context.CancellationToken);
    }
}
