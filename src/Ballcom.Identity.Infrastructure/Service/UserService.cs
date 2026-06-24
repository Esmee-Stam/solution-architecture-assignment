using Ballcom.Identity.Domain.Domain;
using Ballcom.Identity.DomainServices;
using Events.CustomerServiceEvents;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ballcom.Identity.Infrastructure.Service
{
    public class UserService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration,
        IBus bus
    ) : IUserService
    {
        public async Task<bool> RegisterUserAsync(
            string firstName,
            string lastName,
            string? companyName,
            string? phoneNumber,
            string? address,
            string email,
            string password,
            string role)
        {
            if (
                    role != UserRole.Customer 
                    && role != UserRole.WarehouseEmployee 
                    && role != UserRole.Supplier 
                    && role != UserRole.CustomerServiceEmployee
                ) return false;

            var existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser != null) return false;

            var identityUser = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            var result = await userManager.CreateAsync(identityUser, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(identityUser, role);

                if (role == UserRole.Customer)
                {
                    await bus.Publish(new CustomerImportedEvent(
                        firstName,
                        lastName,
                        companyName ?? string.Empty,
                        phoneNumber ?? string.Empty,
                        address ?? string.Empty,
                        identityUser.Id
                    ));
                }

                return true;
            }

            return false;
        }
        
        public async Task<string?> LoginAsync(string email, string password)
        {
            var signInResult = await signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

            if (!signInResult.Succeeded) return null;

            var identityUser = await userManager.FindByEmailAsync(email);

            if (identityUser == null) return null;

            var roles = await userManager.GetRolesAsync(identityUser!);

            var token = GenerateJwtToken(identityUser, roles);

            return token;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }


        private string GenerateJwtToken(IdentityUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var secretKey = configuration["JWT:Secret"];
            var issuer = configuration["JWT:Issuer"];
            var audience = configuration["JWT:Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
