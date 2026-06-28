namespace Events.WarehouseEvents;

public record WarehouseItemEventDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    string Currency
);
