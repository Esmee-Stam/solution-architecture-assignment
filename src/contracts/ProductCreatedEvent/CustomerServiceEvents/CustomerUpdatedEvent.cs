namespace Events.CustomerServiceEvents;

public record CustomerUpdatedEvent(
    Guid CustomerId,
    string FirstName,
    string LastName,
    string? CompanyName,
    string? PhoneNumber,
    string? Address,
    string? IdentityUserId,
    DateTime OccurredAt
);
