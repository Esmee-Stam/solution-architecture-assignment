using Ballcom.Identity.Domain.Domain;

namespace Ballcom.Identity.DomainServices
{
    public interface IUserService
    {
        Task<User?> RegisterUserAsync(string name, string email, string password, string role, string? companyName);
        Task<(User user, string Token)?> LoginAsync(string email, string password);
        Task LogoutAsync();
    }
}
