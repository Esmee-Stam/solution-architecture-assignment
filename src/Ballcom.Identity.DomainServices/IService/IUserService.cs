using Ballcom.Identity.Domain.Domain;

namespace Ballcom.Identity.DomainServices
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(
            string firstName, 
            string lastName, 
            string? companyName, 
            string? phoneNumber, 
            string? address, 
            string email, 
            string password, 
            string role
        );
        Task<string?> LoginAsync(string email, string password);
        Task LogoutAsync();
    }
}
