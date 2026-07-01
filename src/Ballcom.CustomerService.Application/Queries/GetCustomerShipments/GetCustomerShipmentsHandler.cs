using Ballcom.CustomerService.Application.DTOs;

namespace Ballcom.CustomerService.Application.Queries.GetCustomerShipments;

public class GetCustomerShipmentsHandler(ICustomerRepository repository)
{
    public async Task<List<CustomerShipmentDto>> Handle(GetCustomerShipmentsQuery query, CancellationToken cancellationToken = default)
    {
        return await repository.GetCustomerShipmentsAsync(query.CustomerId, cancellationToken);
    }
}
