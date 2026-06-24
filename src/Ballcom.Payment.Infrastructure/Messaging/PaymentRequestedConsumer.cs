using Ballcom.Payment.Application.DTOs;
using Ballcom.Payment.Application.Interfaces;
using Events.PaymentEvents;
using MassTransit;

namespace Ballcom.Payment.Infrastructure.Messaging;

public class PaymentRequestedConsumer(IPaymentReadRepository readRepository)
    : IConsumer<PaymentRequestedEvent>
{
    public async Task Consume(ConsumeContext<PaymentRequestedEvent> context)
    {
        var message = context.Message;

        var payment = new PaymentDto
        {
            PaymentId = message.PaymentId,
            OrderId = message.OrderId,
            CustomerId = message.CustomerId,
            Amount = message.Amount,
            Currency = message.Currency,
            PaymentMethod = message.PaymentMethod,
            Status = "Requested",
            CreatedAt = message.OccurredAt,
            LastUpdatedAt = message.OccurredAt
        };

        await readRepository.UpsertAsync(payment, context.CancellationToken);
    }
}