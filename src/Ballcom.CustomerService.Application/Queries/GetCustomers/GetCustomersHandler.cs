using Ballcom.CustomerService.Application.DTOs;

namespace Ballcom.CustomerService.Application.Queries.GetCustomers;

public class GetCustomersHandler(ICustomerRepository repository)
{
    public async Task<PagedResult<CustomerDto>> Handle(GetCustomersQuery query, CancellationToken cancellationToken = default)
    {
        var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
        var pageSize = query.PageSize is < 1 or > 250 ? 50 : query.PageSize;

        return await repository.GetCustomersAsync(pageNumber, pageSize, query.Search, cancellationToken);
    }
}
