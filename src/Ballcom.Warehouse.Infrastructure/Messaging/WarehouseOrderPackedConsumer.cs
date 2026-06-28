using Ballcom.Warehouse.Application.Interfaces;
using Events.WarehouseEvents;
using MassTransit;

namespace Ballcom.Warehouse.Infrastructure.Messaging;

public class WarehouseOrderPackedConsumer(IWarehouseOrderReadRepository readRepository) : IConsumer<WarehouseOrderPackedEvent>
{
    public async Task Consume(ConsumeContext<WarehouseOrderPackedEvent> context)
    {
        var message = context.Message;

        var warehouseOrder = await readRepository.GetByIdAsync(message.WarehouseOrderId, context.CancellationToken);
        if (warehouseOrder is null) return;

        warehouseOrder.Status = message.Status;
        warehouseOrder.PackedAt = message.OccurredAt;

        await readRepository.UpsertAsync(warehouseOrder, context.CancellationToken);
    }
}
