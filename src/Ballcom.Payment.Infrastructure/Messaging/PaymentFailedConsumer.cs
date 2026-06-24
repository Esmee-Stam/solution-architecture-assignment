using Ballcom.Payment.Application.Interfaces;
using Events.PaymentEvents;
using MassTransit;

namespace Ballcom.Payment.Infrastructure.Messaging;

public class PaymentFailedConsumer(IPaymentReadRepository readRepository)
    : IConsumer<PaymentFailedEvent>
{
    public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        var message = context.Message;

        var existingPayment = await readRepository.GetByIdAsync(
            message.PaymentId,
            context.CancellationToken);

        if (existingPayment is null)
        {
            return;
        }

        existingPayment.Status = "Failed";
        existingPayment.FailureReason = message.Reason;
        existingPayment.LastUpdatedAt = message.OccurredAt;

        await readRepository.UpsertAsync(existingPayment, context.CancellationToken);
    }
}   