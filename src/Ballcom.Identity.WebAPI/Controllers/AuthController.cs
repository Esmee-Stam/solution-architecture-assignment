using Ballcom.Identity.DomainServices;
using Ballcom.Identity.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ballcom.Identity.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterModel registerModel)
    {
        bool isRegistered = await userService.RegisterUserAsync(
           registerModel.FirstName,
           registerModel.LastName,
           registerModel.CompanyName,
           registerModel.PhoneNumber,
           registerModel.Address,
           registerModel.Email,
           registerModel.Password,
           registerModel.Role
        );

        if (isRegistered)
        {
            return Ok(new { Message = "User registered successfully" });
        }

        return BadRequest(new { Message = "User registration failed." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginModel loginModel)
    {
        string? token = await userService.LoginAsync(loginModel.Email, loginModel.Password);
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
