using Ballcom.CustomerService.Domain.Domain;

namespace Ballcom.CustomerService.Application.Services
{
    public class SyncCustomerService(ICustomerRepository repository)
    {
        public async Task UpsertCustomerAsync(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.PhoneNumber)) return;

            var existingCustomer = await repository.GetByPhoneNumberAsync(customer.PhoneNumber);

            if (existingCustomer is null)
            {
                await repository.AddAsync(customer);
            } else
            {
                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.CompanyName = customer.CompanyName;
                existingCustomer.Address = customer.Address;
            }
            await repository.SaveChangesAsync();
        }
    }
}
