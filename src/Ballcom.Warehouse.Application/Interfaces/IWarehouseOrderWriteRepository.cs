using Ballcom.Warehouse.Domain.WarehouseOrders;

namespace Ballcom.Warehouse.Application.Interfaces;

public interface IWarehouseOrderWriteRepository
{
    Task AddAsync(WarehouseOrder warehouseOrder, CancellationToken cancellationToken = default);
    Task<WarehouseOrder?> GetByIdAsync(Guid warehouseOrderId, CancellationToken cancellationToken = default);
    Task<WarehouseOrder?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
