using System.Text.Json;
using Ballcom.Payment.Application.Interfaces;
using Events.PaymentEvents;
using MassTransit;

namespace Ballcom.Payment.Application.Commands.CompletePayment;

public class CompletePaymentHandler(
    IPaymentEventStoreRepository eventStoreRepository,
    IPublishEndpoint publishEndpoint)
{
    public async Task Handle(
        CompletePaymentCommand command,
        CancellationToken cancellationToken = default)
    {
        var events = await eventStoreRepository.GetByPaymentIdAsync(
            command.PaymentId,
            cancellationToken);

        if (!events.Any())
        {
            throw new InvalidOperationException("Payment was not found.");
        }

        if (events.Any(e => e.EventType == nameof(PaymentCompletedEvent)))
        {
            throw new InvalidOperationException("Payment is already completed.");
        }

        if (events.Any(e => e.EventType == nameof(PaymentFailedEvent)))
        {
            throw new InvalidOperationException("Failed payment cannot be completed.");
        }

        var requestedEventDto = events.FirstOrDefault(e => e.EventType == nameof(PaymentRequestedEvent));

        if (requestedEventDto is null)
        {
            throw new InvalidOperationException("PaymentRequested event was not found.");
        }

        var requestedEvent = JsonSerializer.Deserialize<PaymentRequestedEvent>(
            requestedEventDto.EventDataJson);

        if (requestedEvent is null)
        {
            throw new InvalidOperationException("PaymentRequested event could not be deserialized.");
        }

        var paymentCompletedEvent = new PaymentCompletedEvent(
            requestedEvent.PaymentId,
            requestedEvent.OrderId,
            requestedEvent.Amount,
            requestedEvent.Currency,
            DateTime.UtcNow
        );

        await eventStoreRepository.AppendAsync(
            command.PaymentId,
            nameof(PaymentCompletedEvent),
            paymentCompletedEvent,
            cancellationToken);

        await publishEndpoint.Publish(paymentCompletedEvent, cancellationToken);
    }
}