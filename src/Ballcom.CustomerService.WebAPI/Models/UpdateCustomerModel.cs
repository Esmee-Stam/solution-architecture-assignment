namespace Ballcom.CustomerService.WebAPI.Models;

public class UpdateCustomerModel
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? CompanyName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? IdentityUserId { get; set; }
}
