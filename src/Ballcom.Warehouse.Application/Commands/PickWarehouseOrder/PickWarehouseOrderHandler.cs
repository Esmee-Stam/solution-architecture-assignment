using Ballcom.Warehouse.Application.DTOs;
using Ballcom.Warehouse.Application.Interfaces;
using Ballcom.Warehouse.Domain.WarehouseOrders;
using Events.WarehouseEvents;
using MassTransit;

namespace Ballcom.Warehouse.Application.Commands.PickWarehouseOrder;

public class PickWarehouseOrderHandler(
    IWarehouseOrderWriteRepository writeRepository,
    IWarehouseOrderReadRepository readRepository,
    IPublishEndpoint publishEndpoint)
{
    public async Task Handle(PickWarehouseOrderCommand command, CancellationToken cancellationToken = default)
    {
        var warehouseOrder = await writeRepository.GetByIdAsync(command.WarehouseOrderId, cancellationToken)
            ?? throw new InvalidOperationException("Warehouse order was not found.");

        warehouseOrder.PickItems();

        await writeRepository.SaveChangesAsync(cancellationToken);
        await readRepository.UpsertAsync(ToDto(warehouseOrder), cancellationToken);

        await publishEndpoint.Publish(new WarehouseOrderPickedEvent(
            warehouseOrder.Id,
            warehouseOrder.OrderId,
            warehouseOrder.CustomerId,
            warehouseOrder.Status.ToString(),
            DateTime.UtcNow
        ), cancellationToken);
    }

    private static WarehouseOrderDto ToDto(WarehouseOrder warehouseOrder)
    {
        return new WarehouseOrderDto
        {
            Id = warehouseOrder.Id,
            OrderId = warehouseOrder.OrderId,
            CustomerId = warehouseOrder.CustomerId,
            Status = warehouseOrder.Status.ToString(),
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
