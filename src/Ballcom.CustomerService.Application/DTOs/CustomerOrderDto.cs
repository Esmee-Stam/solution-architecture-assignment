namespace Ballcom.CustomerService.Application.DTOs;

public record CustomerOrderDto(
    Guid OrderId,
    Guid CustomerId,
    string Status,
    decimal TotalAmount,
    string Currency,
    string PaymentMethod,
    DateTime? PlacedAt,
    DateTime? LastStatusChangedAt,
    List<CustomerOrderItemDto> Items
);
