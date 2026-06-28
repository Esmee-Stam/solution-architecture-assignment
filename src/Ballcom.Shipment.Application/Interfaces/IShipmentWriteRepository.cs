using ShipmentEntity = Ballcom.Shipment.Domain.Shipments.Shipment;

namespace Ballcom.Shipment.Application.Interfaces;

public interface IShipmentWriteRepository
{
    Task AddAsync(ShipmentEntity shipment, CancellationToken cancellationToken = default);
    Task<ShipmentEntity?> GetByIdAsync(Guid shipmentId, CancellationToken cancellationToken = default);
    Task<ShipmentEntity?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<ShipmentEntity?> GetByWarehouseOrderIdAsync(Guid warehouseOrderId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
