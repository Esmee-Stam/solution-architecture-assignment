using Ballcom.CustomerService.Domain.Domain;
using Events.CustomerServiceEvents;
using MassTransit;

namespace Ballcom.CustomerService.Application.Commands.UpsertImportedCustomer;

public class UpsertImportedCustomerHandler(ICustomerRepository repository, IPublishEndpoint publishEndpoint)
{
    public async Task<UpsertImportedCustomerResult> Handle(UpsertImportedCustomerCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.PhoneNumber))
        {
            throw new InvalidOperationException("Imported customers require a phone number for matching/upserting.");
        }

        var existingCustomer = await repository.GetByPhoneNumberAsync(command.PhoneNumber, cancellationToken);
        var created = existingCustomer is null;

        var customer = existingCustomer ?? Customer.Create(
            command.FirstName,
            command.LastName,
            command.CompanyName,
            command.PhoneNumber,
            command.Address,
            command.IdentityUserId);

        if (existingCustomer is null)
        {
            await repository.AddAsync(customer, cancellationToken);
        }
        else
        {
            customer.UpdateDetails(
                command.FirstName,
                command.LastName,
                command.CompanyName,
                command.PhoneNumber,
                command.Address,
                command.IdentityUserId);
        }

        await repository.SaveChangesAsync(cancellationToken);

        if (created)
        {
            await publishEndpoint.Publish(new CustomerCreatedEvent(
                customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.CompanyName,
                customer.PhoneNumber,
                customer.Address,
                customer.IdentityUserId,
                DateTime.UtcNow), cancellationToken);
        }
        else
        {
            await publishEndpoint.Publish(new CustomerUpdatedEvent(
                customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.CompanyName,
                customer.PhoneNumber,
                customer.Address,
                customer.IdentityUserId,
                DateTime.UtcNow), cancellationToken);
        }

        return new UpsertImportedCustomerResult(customer.Id, created);
    }
}
