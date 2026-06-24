using Ballcom.CustomerService.Domain.Domain;

namespace Ballcom.CustomerService.Application
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByPhoneNumberAsync(string phoneNumber);
        Task AddAsync(Customer customer);
        Task SaveChangesAsync();
    }
}
