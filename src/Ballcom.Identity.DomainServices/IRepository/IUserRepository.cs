using Ballcom.Identity.Domain.Domain;

namespace Ballcom.Identity.DomainServices.IRepository
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
