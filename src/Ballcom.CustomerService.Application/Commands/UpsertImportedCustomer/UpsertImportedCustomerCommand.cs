namespace Ballcom.CustomerService.Application.Commands.UpsertImportedCustomer;

public record UpsertImportedCustomerCommand(
    string FirstName,
    string LastName,
    string? CompanyName,
    string? PhoneNumber,
    string? Address,
    string? IdentityUserId
);
