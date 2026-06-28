namespace Ballcom.Shipment.Application.Commands.CreateShipmentFromPackedWarehouseOrder;

public record CreateShipmentFromPackedWarehouseOrderCommand(
    Guid WarehouseOrderId,
    Guid OrderId,
    Guid CustomerId,
    string Carrier
);
