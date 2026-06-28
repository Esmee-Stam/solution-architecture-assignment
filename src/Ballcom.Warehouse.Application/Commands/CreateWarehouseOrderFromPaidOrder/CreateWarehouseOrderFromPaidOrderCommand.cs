using Events.OrderEvents.Dto;

namespace Ballcom.Warehouse.Application.Commands.CreateWarehouseOrderFromPaidOrder;

public record CreateWarehouseOrderFromPaidOrderCommand(
    Guid OrderId,
    Guid CustomerId,
    string PaymentMethod,
    decimal TotalAmount,
    string Currency,
    List<OrderItemEventDto> Items
);
