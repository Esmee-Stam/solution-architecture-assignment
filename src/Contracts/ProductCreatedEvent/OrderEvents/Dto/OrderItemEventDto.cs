namespace Events.OrderEvents.Dto;

public record OrderItemEventDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    string Currency
);


