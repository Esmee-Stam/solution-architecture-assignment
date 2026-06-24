using System.ComponentModel.DataAnnotations;

namespace Ballcom.Identity.WebAPI.Models
{
    public class UserResponseModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? CompanyName { get; set; }
    }

    public class UserRegisterModel
    {
        [Required(ErrorMessage = "FirstName is required.")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        public required string LastName { get; set; }
        public string? CompanyName { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
        public string? Address { get; set; } = null;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression("Customer|WarehouseEmployee|Supplier|CustomerServiceEmployee", ErrorMessage = "Role must be either 'Customer', 'WarehouseEmployee', 'Supplier' or 'CustomerServiceEmployee.")]
        public required string Role { get; set; }

    }

    public class UserLoginModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public required string Password { get; set; }
    }
}
