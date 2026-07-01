using Ballcom.CustomerService.Application.Services;
using Ballcom.CustomerService.Domain.Domain;
using Events.CustomerServiceEvents;
using MassTransit;

namespace Ballcom.CustomerService.Infrastructure.Messaging;

public class CustomerImportedConsumer(SyncCustomerService service) : IConsumer<CustomerImportedEvent>
{
    public async Task Consume(ConsumeContext<CustomerImportedEvent> context)
    {
        var message = context.Message;

        await service.UpsertCustomerAsync(new Customer
        {
            FirstName = message.FirstName,
            LastName = message.LastName,
            CompanyName = message.CompanyName,
            PhoneNumber = message.PhoneNumber,
            Address = message.Address,
            IdentityUserId = message.IdentityUserId
        });
    }
}
