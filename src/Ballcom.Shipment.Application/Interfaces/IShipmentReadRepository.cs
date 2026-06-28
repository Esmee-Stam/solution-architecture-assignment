using Ballcom.Shipment.Application.DTOs;

namespace Ballcom.Shipment.Application.Interfaces;

public interface IShipmentReadRepository
{
    Task<IReadOnlyList<ShipmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ShipmentDto?> GetByIdAsync(Guid shipmentId, CancellationToken cancellationToken = default);
    Task<ShipmentDto?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<ShipmentDto?> GetByTrackingNumberAsync(string trackingNumber, CancellationToken cancellationToken = default);
    Task UpsertAsync(ShipmentDto shipment, CancellationToken cancellationToken = default);
}
