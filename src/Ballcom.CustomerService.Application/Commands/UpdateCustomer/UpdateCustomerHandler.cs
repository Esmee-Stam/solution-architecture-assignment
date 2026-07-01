using Events.CustomerServiceEvents;
using MassTransit;

namespace Ballcom.CustomerService.Application.Commands.UpdateCustomer;

public class UpdateCustomerHandler(ICustomerRepository repository, IPublishEndpoint publishEndpoint)
{
    public async Task Handle(UpdateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        var customer = await repository.GetByIdEntityAsync(command.CustomerId, cancellationToken)
            ?? throw new InvalidOperationException("Customer was not found.");

        if (!string.IsNullOrWhiteSpace(command.PhoneNumber)
            && await repository.PhoneNumberExistsAsync(command.PhoneNumber, command.CustomerId, cancellationToken))
        {
            throw new InvalidOperationException("A different customer with this phone number already exists.");
        }

        customer.UpdateDetails(
            command.FirstName,
            command.LastName,
            command.CompanyName,
            command.PhoneNumber,
            command.Address,
            command.IdentityUserId);

        await repository.SaveChangesAsync(cancellationToken);

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
}
