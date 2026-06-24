using Ballcom.Payment.Application.Interfaces;
using Events.PaymentEvents;
using MassTransit;

namespace Ballcom.Payment.Infrastructure.Messaging;

public class PaymentCompletedConsumer(IPaymentReadRepository readRepository)
    : IConsumer<PaymentCompletedEvent>
{
    public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
    {
        var message = context.Message;

        var existingPayment = await readRepository.GetByIdAsync(
            message.PaymentId,
            context.CancellationToken);

        if (existingPayment is null)
        {
            return;
        }

        existingPayment.Status = "Completed";
        existingPayment.FailureReason = null;
        existingPayment.LastUpdatedAt = message.OccurredAt;

        await readRepository.UpsertAsync(existingPayment, context.CancellationToken);
    }
}