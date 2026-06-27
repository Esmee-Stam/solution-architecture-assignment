using Events.OrderEvents.Dto;

namespace Events.OrderEvents;

public record OrderPlacedEvent(
    Guid OrderId,
    Guid CustomerId,
    string PaymentMethod,
    decimal TotalAmount,
    string Currency,
    List<OrderItemEventDto> Items
);

