namespace Ballcom.CustomerService.Application.Commands.CreateCustomer;

public record CreateCustomerCommand(
    string FirstName,
    string LastName,
    string? CompanyName,
    string? PhoneNumber,
    string? Address,
    string? IdentityUserId
);
