using Ballcom.Warehouse.Application.DTOs;
using Ballcom.Warehouse.Application.Interfaces;
using Ballcom.Warehouse.Domain.WarehouseOrders;
using Events.WarehouseEvents;
using MassTransit;

namespace Ballcom.Warehouse.Application.Commands.CreateWarehouseOrderFromPaidOrder;

public class CreateWarehouseOrderFromPaidOrderHandler(
    IWarehouseOrderWriteRepository writeRepository,
    IWarehouseOrderReadRepository readRepository,
    IPublishEndpoint publishEndpoint)
{
    public async Task<CreateWarehouseOrderFromPaidOrderResult> Handle(
        CreateWarehouseOrderFromPaidOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var existingWarehouseOrder = await writeRepository.GetByOrderIdAsync(command.OrderId, cancellationToken);
        if (existingWarehouseOrder is not null)
        {
            return new CreateWarehouseOrderFromPaidOrderResult(existingWarehouseOrder.Id);
        }

        var items = command.Items.Select(item => new WarehouseOrderItem(
            item.Id == Guid.Empty ? Guid.NewGuid() : item.Id,
            item.ProductId,
            item.ProductName,
            item.Quantity,
            item.UnitPrice,
            item.Currency
        )).ToList();

        var warehouseOrder = new WarehouseOrder(
            Guid.NewGuid(),
            command.OrderId,
            command.CustomerId,
            command.TotalAmount,
            command.Currency,
            command.PaymentMethod,
            items);

        await writeRepository.AddAsync(warehouseOrder, cancellationToken);
        await writeRepository.SaveChangesAsync(cancellationToken);

        await readRepository.UpsertAsync(ToDto(warehouseOrder), cancellationToken);

        await publishEndpoint.Publish(new WarehouseOrderCreatedEvent(
            warehouseOrder.Id,
            warehouseOrder.OrderId,
            warehouseOrder.CustomerId,
            warehouseOrder.Status.ToString(),
            warehouseOrder.TotalAmount,
            warehouseOrder.Currency,
            warehouseOrder.PaymentMethod,
            warehouseOrder.Items.Select(item => new WarehouseItemEventDto(
                item.Id,
                item.ProductId,
                item.ProductName,
                item.Quantity,
                item.UnitPrice,
                item.Currency
            )).ToList(),
            DateTime.UtcNow
        ), cancellationToken);

        return new CreateWarehouseOrderFromPaidOrderResult(warehouseOrder.Id);
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
