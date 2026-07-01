namespace Events.WarehouseEvents;

public record WarehouseOrderCreatedEvent(
    Guid WarehouseOrderId,
    Guid OrderId,
    Guid CustomerId,
    string Status,
    decimal TotalAmount,
    string Currency,
    string PaymentMethod,
    List<WarehouseItemEventDto> Items,
    DateTime OccurredAt
);
