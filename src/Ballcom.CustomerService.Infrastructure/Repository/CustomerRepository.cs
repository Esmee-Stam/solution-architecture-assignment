using Ballcom.CustomerService.Application;
using Ballcom.CustomerService.Domain.Domain;
using Ballcom.CustomerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.CustomerService.Infrastructure.Repository
{
    public class CustomerRepository(CustomerDbContext context) : ICustomerRepository
    {
        public async Task AddAsync(Customer customer)
        {
            await context.Customers.AddAsync(customer);
            await Task.CompletedTask;
        }

        public async Task<Customer?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await context.Customers.FirstOrDefaultAsync(customer => customer.PhoneNumber == phoneNumber);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
