namespace Ballcom.CustomerService.Application.DTOs;

public record CustomerOrderItemDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    string Currency
);
