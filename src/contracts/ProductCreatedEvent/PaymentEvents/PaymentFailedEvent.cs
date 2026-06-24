namespace Events.PaymentEvents;

public record PaymentFailedEvent(
    Guid PaymentId,
    Guid OrderId,
    string Reason,
    DateTime OccurredAt
);