namespace Ballcom.CustomerService.WebAPI.Models;

public class ImportCustomerModel
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? CompanyName { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? IdentityUserId { get; set; }
}
