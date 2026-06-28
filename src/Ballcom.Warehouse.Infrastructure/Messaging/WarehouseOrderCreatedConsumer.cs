using Ballcom.Warehouse.Application.DTOs;
using Ballcom.Warehouse.Application.Interfaces;
using Events.WarehouseEvents;
using MassTransit;

namespace Ballcom.Warehouse.Infrastructure.Messaging;

public class WarehouseOrderCreatedConsumer(IWarehouseOrderReadRepository readRepository) : IConsumer<WarehouseOrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<WarehouseOrderCreatedEvent> context)
    {
        var message = context.Message;

        await readRepository.UpsertAsync(new WarehouseOrderDto
        {
            Id = message.WarehouseOrderId,
            OrderId = message.OrderId,
            CustomerId = message.CustomerId,
            Status = message.Status,
            TotalAmount = message.TotalAmount,
            Currency = message.Currency,
            PaymentMethod = message.PaymentMethod,
            CreatedAt = message.OccurredAt,
            Items = message.Items.Select(item => new WarehouseOrderItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Currency = item.Currency
            }).ToList()
        }, context.CancellationToken);
    }
}
