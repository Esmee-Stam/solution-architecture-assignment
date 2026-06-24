using System.Text.Json;
using Ballcom.Payment.Application.Interfaces;
using Events.PaymentEvents;
using MassTransit;

namespace Ballcom.Payment.Application.Commands.FailPayment;

public class FailPaymentHandler(
    IPaymentEventStoreRepository eventStoreRepository,
    IPublishEndpoint publishEndpoint)
{
    public async Task Handle(
        FailPaymentCommand command,
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
            throw new InvalidOperationException("Completed payment cannot be failed.");
        }

        if (events.Any(e => e.EventType == nameof(PaymentFailedEvent)))
        {
            throw new InvalidOperationException("Payment is already failed.");
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

        var paymentFailedEvent = new PaymentFailedEvent(
            requestedEvent.PaymentId,
            requestedEvent.OrderId,
            command.Reason,
            DateTime.UtcNow
        );

        await eventStoreRepository.AppendAsync(
            command.PaymentId,
            nameof(PaymentFailedEvent),
            paymentFailedEvent,
            cancellationToken);

        await publishEndpoint.Publish(paymentFailedEvent, cancellationToken);
    }
}