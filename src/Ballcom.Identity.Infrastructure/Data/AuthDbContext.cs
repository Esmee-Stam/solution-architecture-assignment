using Ballcom.Identity.Domain.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Identity.Infrastructure.Data
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options) : IdentityDbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "65b9e0e5-7b56-429a-8e2d-3d445c589a11",
                    Name = UserRole.Customer,
                    NormalizedName = "CUSTOMER",
                    ConcurrencyStamp = "98ce0a8a-03a4-41c1-8d49-f9ebd9fa29fd"
                },
                new IdentityRole
                {
                    Id = "7d8f3b2a-1c5e-4d9f-bf2a-6d8b9a1e2c33",
                    Name = UserRole.CustomerServiceEmployee,
                    NormalizedName = "CUSTOMERSERVICEEMPLOYEE",
                    ConcurrencyStamp = "89f38f3c-f8f6-4849-b935-c3ad98f476a0"
                },
                new IdentityRole
                {
                    Id = "a2b3c4d5-e6f7-4a8b-9c0d-1e2f3a4b5c6d",
                    Name = UserRole.WarehouseEmployee,
                    NormalizedName = "WAREHOUSEEMPLOYEE",
                    ConcurrencyStamp = "71e54a22-310a-4b6d-a112-9cbb82f1470e"
                },
                new IdentityRole
                {
                    Id = "9c2a3b4c-5d6e-4f7a-8b9c-0d1e2f3a4b5c",
                    Name = UserRole.Supplier,
                    NormalizedName = "SUPPLIER",
                    ConcurrencyStamp = "49dbaa1c-6ab5-449b-8ed1-8a6682f2b81f"
                }
            );
        }
    }
}
