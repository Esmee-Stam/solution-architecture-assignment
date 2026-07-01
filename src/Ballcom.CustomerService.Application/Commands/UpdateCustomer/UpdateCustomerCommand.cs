namespace Ballcom.CustomerService.Application.Commands.UpdateCustomer;

public record UpdateCustomerCommand(
    Guid CustomerId,
    string FirstName,
    string LastName,
    string? CompanyName,
    string? PhoneNumber,
    string? Address,
    string? IdentityUserId
);
