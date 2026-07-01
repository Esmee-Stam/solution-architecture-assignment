namespace Events.ShipmentEvents;

public record ShipmentDispatchedEvent(
    Guid ShipmentId,
    Guid OrderId,
    Guid CustomerId,
    string Status,
    string Carrier,
    string TrackingNumber,
    DateTime OccurredAt
);
