namespace Events.OrderEvents;

public record OrderStatusChangedEvent(
    Guid OrderId,
    Guid CustomerId,
    string Status,
    decimal Amount,
    string Currency,
    string PaymentMethod,
    List<OrderItemEventDto> Items,
    DateTime OccurredAt
);


public record OrderItemEventDto(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);

