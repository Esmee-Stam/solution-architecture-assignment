namespace Events.CustomerServiceEvents
{
    public record CustomerImportedEvent(
        string FirstName,
        string LastName,
        string CompanyName,
        string PhoneNumber,
        string Address,
        string? IdentityUserId
    );
}
