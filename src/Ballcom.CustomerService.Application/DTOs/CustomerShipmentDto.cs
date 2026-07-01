namespace Ballcom.CustomerService.Application.DTOs;

public record CustomerShipmentDto(
    Guid ShipmentId,
    Guid WarehouseOrderId,
    Guid OrderId,
    Guid CustomerId,
    string Status,
    string Carrier,
    string TrackingNumber,
    DateTime? CreatedAt,
    DateTime? DispatchedAt,
    DateTime? DeliveredAt,
    DateTime LastUpdatedAt
);
