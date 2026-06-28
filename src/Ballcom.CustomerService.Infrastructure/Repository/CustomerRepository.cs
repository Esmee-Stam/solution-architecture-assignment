using Ballcom.CustomerService.Application;
using Ballcom.CustomerService.Application.DTOs;
using Ballcom.CustomerService.Domain.Domain;
using Ballcom.CustomerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.CustomerService.Infrastructure.Repository
{
    public class CustomerRepository(CustomerServiceDbContext context) : ICustomerRepository
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

        public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id)
        {
            return await context.Customers
                .Where(c => c.Id == id)
                .Select(c => new CustomerDto(c.Id, $"{c.FirstName} {c.LastName}", c.PhoneNumber, c.CompanyName, c.Address))
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResult<CustomerDto>> GetCustomersAsync(int pageNumber, int pageSize)
        {
            var query = context.Customers.AsQueryable();

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(c => c.LastName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CustomerDto(c.Id, $"{c.FirstName} {c.LastName}", c.PhoneNumber, c.CompanyName, c.Address))
                .ToListAsync();

            return new PagedResult<CustomerDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

    }
}
