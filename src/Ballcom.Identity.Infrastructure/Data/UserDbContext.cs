using Ballcom.Identity.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Identity.Infrastructure.Data
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
    {
        public const string SqlSchema = "UserDB";

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(SqlSchema);

            builder.Entity<User>().HasIndex(user => user.Email).IsUnique();

            base.OnModelCreating(builder);
        }
    }
}
