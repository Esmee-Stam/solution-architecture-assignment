using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Ballcom.Identity.Infrastructure.Data
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options) : IdentityDbContext(options)
    {
        public const string SqlSchema = "AuthenticationDB";

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(SqlSchema);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "65b9e0e5-7b56-429a-8e2d-3d445c589a11",
                    Name = "Customer",
                    NormalizedName = "CUSTOMER",
                    ConcurrencyStamp = "98ce0a8a-03a4-41c1-8d49-f9ebd9fa29fd"
                },
                new IdentityRole
                {
                    Id = "7d8f3b2a-1c5e-4d9f-bf2a-6d8b9a1e2c33",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE",
                    ConcurrencyStamp = "89f38f3c-f8f6-4849-b935-c3ad98f476a0"
                },
                new IdentityRole
                {
                    Id = "9c2a3b4c-5d6e-4f7a-8b9c-0d1e2f3a4b5c",
                    Name = "Supplier",
                    NormalizedName = "SUPPLIER",
                    ConcurrencyStamp = "49dbaa1c-6ab5-449b-8ed1-8a6682f2b81f"
                }
            );

            base.OnModelCreating(builder);
        }
    }
}
