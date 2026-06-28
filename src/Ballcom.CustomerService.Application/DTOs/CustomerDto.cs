namespace Ballcom.CustomerService.Application.DTOs;

public record CustomerDto
(
     Guid Id,
     string Name,
     string PhoneNumber,
    string CompanyName,
    string Address
);
