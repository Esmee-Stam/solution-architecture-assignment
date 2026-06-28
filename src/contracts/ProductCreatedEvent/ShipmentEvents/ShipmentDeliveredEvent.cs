namespace Events.ShipmentEvents;

public record ShipmentDeliveredEvent(
    Guid ShipmentId,
    Guid OrderId,
    Guid CustomerId,
    string Status,
    string Carrier,
    string TrackingNumber,
    DateTime OccurredAt
);
