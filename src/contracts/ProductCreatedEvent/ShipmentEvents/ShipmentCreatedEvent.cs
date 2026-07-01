namespace Events.ShipmentEvents;

public record ShipmentCreatedEvent(
    Guid ShipmentId,
    Guid WarehouseOrderId,
    Guid OrderId,
    Guid CustomerId,
    string Status,
    string Carrier,
    string TrackingNumber,
    DateTime OccurredAt
);
