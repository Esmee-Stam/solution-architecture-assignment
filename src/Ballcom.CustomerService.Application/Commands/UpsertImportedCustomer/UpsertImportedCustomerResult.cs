namespace Ballcom.CustomerService.Application.Commands.UpsertImportedCustomer;

public record UpsertImportedCustomerResult(Guid CustomerId, bool Created);
