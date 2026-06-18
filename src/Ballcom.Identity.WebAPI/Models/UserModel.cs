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
        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression("Customer|Employee|Supplier", ErrorMessage = "Role must be either 'Customer', 'Employee', or 'Supplier'.")]
        public required string Role { get; set; }

        public string? CompanyName { get; set; } = null;
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
