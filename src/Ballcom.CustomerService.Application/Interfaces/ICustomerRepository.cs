using Ballcom.CustomerService.Application.DTOs;
using Ballcom.CustomerService.Domain.Domain;

namespace Ballcom.CustomerService.Application
{
    public interface ICustomerRepository
    {
        Task<PagedResult<CustomerDto>> GetCustomersAsync(int pageNumber, int pageSize);
        Task<CustomerDto?> GetCustomerByIdAsync(Guid id);
        Task<Customer?> GetByPhoneNumberAsync(string phoneNumber);
        Task AddAsync(Customer customer);
        Task SaveChangesAsync();
    }
}
