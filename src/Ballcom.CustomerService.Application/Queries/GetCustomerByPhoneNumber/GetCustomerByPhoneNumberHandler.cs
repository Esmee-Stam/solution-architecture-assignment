using Ballcom.CustomerService.Application.DTOs;

namespace Ballcom.CustomerService.Application.Queries.GetCustomerByPhoneNumber;

public class GetCustomerByPhoneNumberHandler(ICustomerRepository repository)
{
    public async Task<CustomerDto?> Handle(GetCustomerByPhoneNumberQuery query, CancellationToken cancellationToken = default)
    {
        return await repository.GetCustomerByPhoneNumberAsync(query.PhoneNumber, cancellationToken);
    }
}
