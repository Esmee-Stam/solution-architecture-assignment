using Ballcom.Identity.Domain.Domain;
using Ballcom.Identity.DomainServices;
using Ballcom.Identity.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ballcom.Identity.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterModel registerModel)
    {
        User? user = await userService.RegisterUserAsync(
           registerModel.Name,
           registerModel.Email,
           registerModel.Password,
           registerModel.Role,
           registerModel.CompanyName
        );

        if (user != null)
        {
           return Ok(new
           {
               Message = "User registered successfully",
               User = new
               {
                   user.Id,
                   user.Name,
                   user.Email,
                   user.Role,
                   user.CompanyName
               }
           });
        }
        else
        {
           return BadRequest(new { Message = "User registration failed." });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginModel loginModel)
    {
        var result = await userService.LoginAsync(loginModel.Email, loginModel.Password);
        if (result != null)
        {
            var (user, token) = result.Value;
            return Ok(new
            {
                Message = "Login successful",
                Token = token,
                User = new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Role,
                    user.CompanyName
                }
            });
        }
        else
        {
            return Unauthorized(new { Message = "Invalid email or password." });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await userService.LogoutAsync();
        return Ok(new { Message = "Logout successful" });
    }
}
