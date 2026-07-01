using Ballcom.CustomerService.Application;
using Ballcom.CustomerService.Domain.Domain;
using Events.OrderEvents;
using MassTransit;

namespace Ballcom.CustomerService.Infrastructure.Messaging;

public class OrderPlacedConsumer(ICustomerRepository repository) : IConsumer<OrderPlacedEvent>
{
    public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
    {
        var message = context.Message;

        var order = new CustomerOrder
        {
            OrderId = message.OrderId,
            CustomerId = message.CustomerId,
            Status = "Placed",
            TotalAmount = message.TotalAmount,
            Currency = message.Currency,
            PaymentMethod = message.PaymentMethod,
            PlacedAt = DateTime.UtcNow,
            LastStatusChangedAt = DateTime.UtcNow,
            Items = message.Items.Select(item => new CustomerOrderItem
            {
                Id = item.Id,
                OrderId = message.OrderId,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Currency = item.Currency
            }).ToList()
        };

        await repository.UpsertOrderAsync(order, context.CancellationToken);
        await repository.SaveChangesAsync(context.CancellationToken);
    }
}
