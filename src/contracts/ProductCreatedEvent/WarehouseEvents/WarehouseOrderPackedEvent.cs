namespace Events.WarehouseEvents;

public record WarehouseOrderPackedEvent(
    Guid WarehouseOrderId,
    Guid OrderId,
    Guid CustomerId,
    string Status,
    DateTime OccurredAt
);
