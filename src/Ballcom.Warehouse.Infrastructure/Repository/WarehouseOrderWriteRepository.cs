using Ballcom.Warehouse.Application.Interfaces;
using Ballcom.Warehouse.Domain.WarehouseOrders;
using Ballcom.Warehouse.Infrastructure.Data.Write;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Warehouse.Infrastructure.Repository;

public class WarehouseOrderWriteRepository(WarehouseWriteDbContext dbContext) : IWarehouseOrderWriteRepository
{
    public async Task AddAsync(WarehouseOrder warehouseOrder, CancellationToken cancellationToken = default)
    {
        await dbContext.WarehouseOrders.AddAsync(warehouseOrder, cancellationToken);
    }

    public async Task<WarehouseOrder?> GetByIdAsync(Guid warehouseOrderId, CancellationToken cancellationToken = default)
    {
        return await dbContext.WarehouseOrders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == warehouseOrderId, cancellationToken);
    }

    public async Task<WarehouseOrder?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await dbContext.WarehouseOrders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
