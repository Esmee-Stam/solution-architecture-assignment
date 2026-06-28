using Ballcom.Shipment.Application.Interfaces;
using Ballcom.Shipment.Infrastructure.Data.Write;
using Microsoft.EntityFrameworkCore;
using ShipmentEntity = Ballcom.Shipment.Domain.Shipments.Shipment;

namespace Ballcom.Shipment.Infrastructure.Repository;

public class ShipmentWriteRepository(ShipmentWriteDbContext dbContext) : IShipmentWriteRepository
{
    public async Task AddAsync(ShipmentEntity shipment, CancellationToken cancellationToken = default)
    {
        await dbContext.Shipments.AddAsync(shipment, cancellationToken);
    }

    public async Task<ShipmentEntity?> GetByIdAsync(Guid shipmentId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Shipments.FirstOrDefaultAsync(x => x.Id == shipmentId, cancellationToken);
    }

    public async Task<ShipmentEntity?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Shipments.FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);
    }

    public async Task<ShipmentEntity?> GetByWarehouseOrderIdAsync(Guid warehouseOrderId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Shipments.FirstOrDefaultAsync(x => x.WarehouseOrderId == warehouseOrderId, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
