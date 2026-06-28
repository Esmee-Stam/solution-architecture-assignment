using Ballcom.Shipment.Application.DTOs;
using Ballcom.Shipment.Application.Interfaces;
using Ballcom.Shipment.Infrastructure.Data.Read;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Shipment.Infrastructure.Repository;

public class ShipmentReadRepository(ShipmentReadDbContext dbContext) : IShipmentReadRepository
{
    public async Task<IReadOnlyList<ShipmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var shipments = await dbContext.Shipments
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return shipments.Select(ToDto).ToList();
    }

    public async Task<ShipmentDto?> GetByIdAsync(Guid shipmentId, CancellationToken cancellationToken = default)
    {
        var shipment = await dbContext.Shipments.FirstOrDefaultAsync(x => x.Id == shipmentId, cancellationToken);
        return shipment is null ? null : ToDto(shipment);
    }

    public async Task<ShipmentDto?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var shipment = await dbContext.Shipments.FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);
        return shipment is null ? null : ToDto(shipment);
    }

    public async Task<ShipmentDto?> GetByTrackingNumberAsync(string trackingNumber, CancellationToken cancellationToken = default)
    {
        var normalizedTrackingNumber = trackingNumber.Trim().ToUpperInvariant();
        var shipment = await dbContext.Shipments.FirstOrDefaultAsync(x => x.TrackingNumber == normalizedTrackingNumber, cancellationToken);
        return shipment is null ? null : ToDto(shipment);
    }

    public async Task UpsertAsync(ShipmentDto shipment, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.Shipments.FirstOrDefaultAsync(x => x.Id == shipment.Id, cancellationToken);

        if (existing is null)
        {
            existing = new ShipmentReadModel
            {
                Id = shipment.Id,
                WarehouseOrderId = shipment.WarehouseOrderId,
                OrderId = shipment.OrderId,
                CustomerId = shipment.CustomerId,
                Status = shipment.Status,
                Carrier = shipment.Carrier,
                TrackingNumber = shipment.TrackingNumber,
                CreatedAt = shipment.CreatedAt,
                DispatchedAt = shipment.DispatchedAt,
                DeliveredAt = shipment.DeliveredAt
            };

            dbContext.Shipments.Add(existing);
        }
        else
        {
            existing.WarehouseOrderId = shipment.WarehouseOrderId;
            existing.OrderId = shipment.OrderId;
            existing.CustomerId = shipment.CustomerId;
            existing.Status = shipment.Status;
            existing.Carrier = shipment.Carrier;
            existing.TrackingNumber = shipment.TrackingNumber;
            existing.CreatedAt = shipment.CreatedAt;
            existing.DispatchedAt = shipment.DispatchedAt;
            existing.DeliveredAt = shipment.DeliveredAt;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static ShipmentDto ToDto(ShipmentReadModel shipment)
    {
        return new ShipmentDto
        {
            Id = shipment.Id,
            WarehouseOrderId = shipment.WarehouseOrderId,
            OrderId = shipment.OrderId,
            CustomerId = shipment.CustomerId,
            Status = shipment.Status,
            Carrier = shipment.Carrier,
            TrackingNumber = shipment.TrackingNumber,
            CreatedAt = shipment.CreatedAt,
            DispatchedAt = shipment.DispatchedAt,
            DeliveredAt = shipment.DeliveredAt
        };
    }
}
