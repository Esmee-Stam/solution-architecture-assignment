namespace Events.OrderEvents.Dto;

public record OrderItemEventDto(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);


