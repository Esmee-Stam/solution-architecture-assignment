using Ballcom.Warehouse.Application.Interfaces;
using Events.WarehouseEvents;
using MassTransit;

namespace Ballcom.Warehouse.Infrastructure.Messaging;

public class WarehouseOrderPickedConsumer(IWarehouseOrderReadRepository readRepository) : IConsumer<WarehouseOrderPickedEvent>
{
    public async Task Consume(ConsumeContext<WarehouseOrderPickedEvent> context)
    {
        var message = context.Message;

        var warehouseOrder = await readRepository.GetByIdAsync(message.WarehouseOrderId, context.CancellationToken);
        if (warehouseOrder is null) return;

        warehouseOrder.Status = message.Status;
        warehouseOrder.PickedAt = message.OccurredAt;

        await readRepository.UpsertAsync(warehouseOrder, context.CancellationToken);
    }
}
