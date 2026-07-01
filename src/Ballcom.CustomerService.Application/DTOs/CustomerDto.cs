namespace Ballcom.CustomerService.Application.DTOs;

public record CustomerDto
(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    string? PhoneNumber,
    string? CompanyName,
    string? Address,
    string? IdentityUserId
);
