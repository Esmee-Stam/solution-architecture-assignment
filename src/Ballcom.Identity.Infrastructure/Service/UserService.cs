using Ballcom.Identity.Domain.Domain;
using Ballcom.Identity.DomainServices;
using Ballcom.Identity.DomainServices.IRepository;
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
        IUserRepository userRepository,
        IConfiguration configuration
    ) : IUserService
    {
        public async Task<User?> RegisterUserAsync(string name, string email, string password, string role, string? companyName)
        {
            if (role != UserRole.Customer && role != UserRole.Employee && role != UserRole.Supplier) return null;

            var existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser != null) return null;

            var identityUser = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            var result = await userManager.CreateAsync(identityUser, password);

            if (result.Succeeded)
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Email = email,
                    Role = role,
                    CompanyName = companyName,
                    IdentityUserId = identityUser.Id
                };

                await userRepository.AddUserAsync(user);
                await userManager.AddToRoleAsync(identityUser, role);
                return user;
            }

            return null;
        }
        
        public async Task<(User user, string Token)?> LoginAsync(string email, string password)
        {
            var signInResult = await signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

            if (!signInResult.Succeeded) return null;

            var identityUser = await userManager.FindByEmailAsync(email);

            if (identityUser == null) return null;

            var roles = await userManager.GetRolesAsync(identityUser!);

            var user = await userRepository.GetUserByEmailAsync(email);

            if (user == null) return null;

            var token = GenerateJwtToken(identityUser, roles);

            return (user, token);
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
