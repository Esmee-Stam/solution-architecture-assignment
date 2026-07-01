using Ballcom.CustomerService.Application.Commands.UpsertImportedCustomer;
using Events.CustomerServiceEvents;
using MassTransit;

namespace Ballcom.CustomerService.Infrastructure.Messaging;

public class CustomerImportedConsumer(UpsertImportedCustomerHandler handler) : IConsumer<CustomerImportedEvent>
{
    public async Task Consume(ConsumeContext<CustomerImportedEvent> context)
    {
        var message = context.Message;

        await handler.Handle(new UpsertImportedCustomerCommand(
            message.FirstName,
            message.LastName,
            message.CompanyName,
            message.PhoneNumber,
            message.Address,
            message.IdentityUserId), context.CancellationToken);
    }
}
