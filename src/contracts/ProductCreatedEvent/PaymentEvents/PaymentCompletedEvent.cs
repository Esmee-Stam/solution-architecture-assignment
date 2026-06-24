namespace Events.PaymentEvents;

public record PaymentCompletedEvent(
    Guid PaymentId,
    Guid OrderId,
    decimal Amount,
    string Currency,
    DateTime OccurredAt
);