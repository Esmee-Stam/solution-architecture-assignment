using Ballcom.Identity.Domain.Domain;
using Ballcom.Identity.DomainServices.IRepository;
using Ballcom.Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Identity.Infrastructure.Repository
{
    public class UserRepository(UserDbContext context) : IUserRepository
    {
        public async Task AddUserAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }
    }
}
