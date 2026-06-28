using System.ComponentModel.DataAnnotations;

namespace Ballcom.CustomerService.Domain.Domain
{
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
    }
}
