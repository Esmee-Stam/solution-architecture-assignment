using System.ComponentModel.DataAnnotations;

namespace Ballcom.CustomerService.Domain.Domain;

public class Customer
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(100)]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public required string LastName { get; set; }

    public string? CompanyName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? IdentityUserId { get; set; }

    public void UpdateDetails(
        string firstName,
        string lastName,
        string? companyName,
        string? phoneNumber,
        string? address,
        string? identityUserId)
    {
        FirstName = NormalizeRequired(firstName, nameof(firstName));
        LastName = NormalizeRequired(lastName, nameof(lastName));
        CompanyName = NormalizeOptional(companyName);
        PhoneNumber = NormalizeOptional(phoneNumber);
        Address = NormalizeOptional(address);
        IdentityUserId = NormalizeOptional(identityUserId);
    }

    public static Customer Create(
        string firstName,
        string lastName,
        string? companyName,
        string? phoneNumber,
        string? address,
        string? identityUserId = null)
    {
        return new Customer
        {
            FirstName = NormalizeRequired(firstName, nameof(firstName)),
            LastName = NormalizeRequired(lastName, nameof(lastName)),
            CompanyName = NormalizeOptional(companyName),
            PhoneNumber = NormalizeOptional(phoneNumber),
            Address = NormalizeOptional(address),
            IdentityUserId = NormalizeOptional(identityUserId)
        };
    }

    private static string NormalizeRequired(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{fieldName} is required.", fieldName);
        }

        return value.Trim();
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
