namespace Events.WarehouseEvents;

public record WarehouseOrderPickedEvent(
    Guid WarehouseOrderId,
    Guid OrderId,
    Guid CustomerId,
    string Status,
    DateTime OccurredAt
);
