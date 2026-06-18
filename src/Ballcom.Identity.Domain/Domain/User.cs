using System.ComponentModel.DataAnnotations;

namespace Ballcom.Identity.Domain.Domain
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Role { get; set; }

        public string? CompanyName { get; set; }

        public string? IdentityUserId { get; set; }
    }
}
