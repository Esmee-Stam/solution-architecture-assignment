using Ballcom.CustomerService.Application.DTOs;

namespace Ballcom.CustomerService.Application.Queries.GetCustomerById;

public class GetCustomerByIdHandler(ICustomerRepository repository)
{
    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await repository.GetCustomerByIdAsync(query.CustomerId, cancellationToken);
    }
}
