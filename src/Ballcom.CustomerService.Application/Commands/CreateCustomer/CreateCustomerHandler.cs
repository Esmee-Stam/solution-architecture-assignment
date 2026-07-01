using Ballcom.CustomerService.Domain.Domain;
using Events.CustomerServiceEvents;
using MassTransit;

namespace Ballcom.CustomerService.Application.Commands.CreateCustomer;

public class CreateCustomerHandler(ICustomerRepository repository, IPublishEndpoint publishEndpoint)
{
    public async Task<CreateCustomerResult> Handle(CreateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(command.PhoneNumber)
            && await repository.PhoneNumberExistsAsync(command.PhoneNumber, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException("A customer with this phone number already exists.");
        }

        var customer = Customer.Create(
            command.FirstName,
            command.LastName,
            command.CompanyName,
            command.PhoneNumber,
            command.Address,
            command.IdentityUserId);

        await repository.AddAsync(customer, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new CustomerCreatedEvent(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.CompanyName,
            customer.PhoneNumber,
            customer.Address,
            customer.IdentityUserId,
            DateTime.UtcNow), cancellationToken);

        return new CreateCustomerResult(customer.Id);
    }
}
