using Ballcom.Warehouse.Application.DTOs;

namespace Ballcom.Warehouse.Application.Interfaces;

public interface IWarehouseOrderReadRepository
{
    Task<IReadOnlyList<WarehouseOrderDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<WarehouseOrderDto?> GetByIdAsync(Guid warehouseOrderId, CancellationToken cancellationToken = default);
    Task<WarehouseOrderDto?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task UpsertAsync(WarehouseOrderDto warehouseOrder, CancellationToken cancellationToken = default);
}
