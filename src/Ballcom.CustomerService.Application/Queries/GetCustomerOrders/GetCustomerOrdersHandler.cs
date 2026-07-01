using Ballcom.CustomerService.Application.DTOs;

namespace Ballcom.CustomerService.Application.Queries.GetCustomerOrders;

public class GetCustomerOrdersHandler(ICustomerRepository repository)
{
    public async Task<List<CustomerOrderDto>> Handle(GetCustomerOrdersQuery query, CancellationToken cancellationToken = default)
    {
        return await repository.GetCustomerOrdersAsync(query.CustomerId, cancellationToken);
    }
}
