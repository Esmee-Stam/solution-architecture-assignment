using Ballcom.CustomerService.Application.DTOs;

namespace Ballcom.CustomerService.Application.Queries.GetCustomerOverview;

public class GetCustomerOverviewHandler(ICustomerRepository repository)
{
    public async Task<CustomerOverviewDto?> Handle(GetCustomerOverviewQuery query, CancellationToken cancellationToken = default)
    {
        return await repository.GetCustomerOverviewAsync(query.CustomerId, cancellationToken);
    }
}
