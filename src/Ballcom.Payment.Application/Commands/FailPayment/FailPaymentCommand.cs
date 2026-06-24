namespace Ballcom.Payment.Application.Commands.FailPayment;

public record FailPaymentCommand(
    Guid PaymentId,
    string Reason
);