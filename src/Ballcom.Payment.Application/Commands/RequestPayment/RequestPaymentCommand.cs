namespace Ballcom.Payment.Application.Commands.RequestPayment;

public record RequestPaymentCommand(
    Guid OrderId,
    Guid CustomerId,
    decimal Amount,
    string Currency,
    string PaymentMethod
);