using Ballcom.Payment.Application.Interfaces;
using Ballcom.Payment.Domain.Payments;
using Ballcom.Payment.Domain.ValueObjects;
using Events.PaymentEvents;
using MassTransit;

namespace Ballcom.Payment.Application.Commands.RequestPayment;

public class RequestPaymentHandler(
    IPaymentEventStoreRepository eventStoreRepository,
    IPublishEndpoint publishEndpoint)
{
    public async Task<RequestPaymentResult> Handle(
        RequestPaymentCommand command,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<PaymentMethod>(command.PaymentMethod, true, out var paymentMethod))
        {
            throw new ArgumentException("Invalid payment method. Use ForwardPay or AfterPay.");
        }

        var payment = new Domain.Payments.Payment(
            Guid.NewGuid(),
            command.OrderId,
            command.CustomerId,
            new Money(command.Amount, command.Currency),
            paymentMethod
        );

        var paymentRequestedEvent = new PaymentRequestedEvent(
            payment.PaymentId,
            payment.OrderId,
            payment.CustomerId,
            payment.Amount.Amount,
            payment.Amount.Currency,
            payment.Method.ToString(),
            DateTime.UtcNow
        );

        await eventStoreRepository.AppendAsync(
            payment.PaymentId,
            nameof(PaymentRequestedEvent),
            paymentRequestedEvent,
            cancellationToken);

        await publishEndpoint.Publish(paymentRequestedEvent, cancellationToken);

        return new RequestPaymentResult(payment.PaymentId);
    }
}