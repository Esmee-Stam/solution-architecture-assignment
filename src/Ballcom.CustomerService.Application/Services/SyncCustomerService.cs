using Ballcom.CustomerService.Domain.Domain;

namespace Ballcom.CustomerService.Application.Services;

public class SyncCustomerService(ICustomerRepository repository)
{
    public async Task UpsertCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(customer.PhoneNumber)) return;

        var existingCustomer = await repository.GetByPhoneNumberAsync(customer.PhoneNumber, cancellationToken);

        if (existingCustomer is null)
        {
            await repository.AddAsync(customer, cancellationToken);
        }
        else
        {
            existingCustomer.UpdateDetails(
                customer.FirstName,
                customer.LastName,
                customer.CompanyName,
                customer.PhoneNumber,
                customer.Address,
                customer.IdentityUserId);
        }

        await repository.SaveChangesAsync(cancellationToken);
    }
}
