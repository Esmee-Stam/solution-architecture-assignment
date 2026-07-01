using Ballcom.CustomerService.Application.DTOs;
using Ballcom.CustomerService.Domain.Domain;

namespace Ballcom.CustomerService.Application;

public interface ICustomerRepository
{
    Task<PagedResult<CustomerDto>> GetCustomersAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default);
    Task<CustomerDto?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CustomerDto?> GetCustomerByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<Customer?> GetByIdEntityAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Customer?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<bool> PhoneNumberExistsAsync(string phoneNumber, Guid? excludedCustomerId = null, CancellationToken cancellationToken = default);
    Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
