using Ballcom.Warehouse.Application.DTOs;
using Ballcom.Warehouse.Application.Interfaces;
using Ballcom.Warehouse.Domain.WarehouseOrders;
using Events.OrderEvents;
using Events.OrderEvents.Dto;
using Events.WarehouseEvents;
using MassTransit;

namespace Ballcom.Warehouse.Application.Commands.PackWarehouseOrder;

public class PackWarehouseOrderHandler(
    IWarehouseOrderWriteRepository writeRepository,
    IWarehouseOrderReadRepository readRepository,
    IPublishEndpoint publishEndpoint)
{
    public async Task Handle(PackWarehouseOrderCommand command, CancellationToken cancellationToken = default)
    {
        var warehouseOrder = await writeRepository.GetByIdAsync(command.WarehouseOrderId, cancellationToken)
            ?? throw new InvalidOperationException("Warehouse order was not found.");

        warehouseOrder.Pack();

        await writeRepository.SaveChangesAsync(cancellationToken);
        await readRepository.UpsertAsync(ToDto(warehouseOrder), cancellationToken);

        await publishEndpoint.Publish(new WarehouseOrderPackedEvent(
            warehouseOrder.Id,
            warehouseOrder.OrderId,
            warehouseOrder.CustomerId,
            warehouseOrder.Status.ToString(),
            DateTime.UtcNow
        ), cancellationToken);

        await publishEndpoint.Publish(new OrderStatusChangedEvent(
            warehouseOrder.OrderId,
            warehouseOrder.CustomerId,
            "Packed",
            warehouseOrder.TotalAmount,
            warehouseOrder.Currency,
            warehouseOrder.PaymentMethod,
            warehouseOrder.Items.Select(item => new OrderItemEventDto(
                item.Id,
                item.ProductId,
                item.ProductName,
                item.Quantity,
                item.UnitPrice,
                item.Currency
            )).ToList(),
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
