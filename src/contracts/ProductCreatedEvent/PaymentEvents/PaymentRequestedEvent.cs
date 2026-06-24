namespace Events.PaymentEvents;

public record PaymentRequestedEvent(
    Guid PaymentId,
    Guid OrderId,
    Guid CustomerId,
    decimal Amount,
    string Currency,
    string PaymentMethod,
    DateTime OccurredAt
);