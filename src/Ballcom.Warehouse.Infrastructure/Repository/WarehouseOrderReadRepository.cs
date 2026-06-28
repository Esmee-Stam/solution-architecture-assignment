using Ballcom.Warehouse.Application.DTOs;
using Ballcom.Warehouse.Application.Interfaces;
using Ballcom.Warehouse.Infrastructure.Data.Read;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Warehouse.Infrastructure.Repository;

public class WarehouseOrderReadRepository(WarehouseReadDbContext dbContext) : IWarehouseOrderReadRepository
{
    public async Task<IReadOnlyList<WarehouseOrderDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var warehouseOrders = await dbContext.WarehouseOrders
            .Include(x => x.Items)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return warehouseOrders.Select(ToDto).ToList();
    }

    public async Task<WarehouseOrderDto?> GetByIdAsync(Guid warehouseOrderId, CancellationToken cancellationToken = default)
    {
        var order = await dbContext.WarehouseOrders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == warehouseOrderId, cancellationToken);

        return order is null ? null : ToDto(order);
    }

    public async Task<WarehouseOrderDto?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await dbContext.WarehouseOrders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);

        return order is null ? null : ToDto(order);
    }

    public async Task UpsertAsync(WarehouseOrderDto warehouseOrder, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.WarehouseOrders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == warehouseOrder.Id, cancellationToken);

        if (existing is null)
        {
            existing = new WarehouseOrderReadModel
            {
                Id = warehouseOrder.Id,
                OrderId = warehouseOrder.OrderId,
                CustomerId = warehouseOrder.CustomerId,
                Status = warehouseOrder.Status,
                TotalAmount = warehouseOrder.TotalAmount,
                Currency = warehouseOrder.Currency,
                PaymentMethod = warehouseOrder.PaymentMethod,
                CreatedAt = warehouseOrder.CreatedAt,
                PickedAt = warehouseOrder.PickedAt,
                PackedAt = warehouseOrder.PackedAt,
                Items = warehouseOrder.Items.Select(item => new WarehouseOrderItemReadModel
                {
                    Id = item.Id,
                    WarehouseOrderId = warehouseOrder.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Currency = item.Currency
                }).ToList()
            };

            dbContext.WarehouseOrders.Add(existing);
        }
        else
        {
            existing.OrderId = warehouseOrder.OrderId;
            existing.CustomerId = warehouseOrder.CustomerId;
            existing.Status = warehouseOrder.Status;
            existing.TotalAmount = warehouseOrder.TotalAmount;
            existing.Currency = warehouseOrder.Currency;
            existing.PaymentMethod = warehouseOrder.PaymentMethod;
            existing.CreatedAt = warehouseOrder.CreatedAt;
            existing.PickedAt = warehouseOrder.PickedAt;
            existing.PackedAt = warehouseOrder.PackedAt;

            // Items do not change during the warehouse flow. The existing item list is left intact
            // to avoid tracking duplicate entities with the same keys during status-only updates.
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static WarehouseOrderDto ToDto(WarehouseOrderReadModel warehouseOrder)
    {
        return new WarehouseOrderDto
        {
            Id = warehouseOrder.Id,
            OrderId = warehouseOrder.OrderId,
            CustomerId = warehouseOrder.CustomerId,
            Status = warehouseOrder.Status,
            TotalAmount = warehouseOrder.TotalAmount,
            Currency = warehouseOrder.Currency,
            PaymentMethod = warehouseOrder.PaymentMethod,
            CreatedAt = warehouseOrder.CreatedAt,
            PickedAt = warehouseOrder.PickedAt,
            PackedAt = warehouseOrder.PackedAt,
            Items = warehouseOrder.Items.Select(item => new WarehouseOrderItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Currency = item.Currency
            }).ToList()
        };
    }
}
