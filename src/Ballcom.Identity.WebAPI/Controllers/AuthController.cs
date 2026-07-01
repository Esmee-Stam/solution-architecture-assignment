using Ballcom.Identity.Domain.Domain;
using Ballcom.Identity.DomainServices;
using Ballcom.Identity.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ballcom.Identity.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService) : ControllerBase
{
    private static readonly HashSet<string> AllowedRoles = new(StringComparer.Ordinal)
    {
        UserRole.Customer,
        UserRole.WarehouseEmployee,
        UserRole.Supplier,
        UserRole.CustomerServiceEmployee
    };

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterModel registerModel)
    {
        var role = registerModel.Role.Trim();

        if (!AllowedRoles.Contains(role))
        {
            return BadRequest(new
            {
                Message = "Invalid role.",
                AllowedRoles = AllowedRoles.ToArray()
            });
        }

        bool isRegistered = await userService.RegisterUserAsync(
           registerModel.FirstName.Trim(),
           registerModel.LastName.Trim(),
           string.IsNullOrWhiteSpace(registerModel.CompanyName) ? null : registerModel.CompanyName.Trim(),
           string.IsNullOrWhiteSpace(registerModel.PhoneNumber) ? null : registerModel.PhoneNumber.Trim(),
           string.IsNullOrWhiteSpace(registerModel.Address) ? null : registerModel.Address.Trim(),
           registerModel.Email.Trim(),
           registerModel.Password,
           role
        );

        if (isRegistered)
        {
            return Ok(new { Message = "User registered successfully" });
        }

        return BadRequest(new { Message = "User registration failed. The email may already exist or the password may not meet the Identity requirements." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginModel loginModel)
    {
        string? token = await userService.LoginAsync(loginModel.Email.Trim(), loginModel.Password);
        if (!string.IsNullOrEmpty(token))
        {
            return Ok(new
            {
                Message = "Login successful",
                Token = token
            });
        }

        return Unauthorized(new { Message = "Invalid email or password." });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await userService.LogoutAsync();
        return Ok(new { Message = "Logout successful" });
    }
}
