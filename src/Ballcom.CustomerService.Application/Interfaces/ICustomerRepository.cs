using Ballcom.CustomerService.Application.DTOs;
using Ballcom.CustomerService.Domain.Domain;

namespace Ballcom.CustomerService.Application;

public interface ICustomerRepository
{
    Task<PagedResult<CustomerDto>> GetCustomersAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default);
    Task<CustomerDto?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CustomerDto?> GetCustomerByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<List<CustomerOrderDto>> GetCustomerOrdersAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<List<CustomerShipmentDto>> GetCustomerShipmentsAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<CustomerOverviewDto?> GetCustomerOverviewAsync(Guid customerId, CancellationToken cancellationToken = default);

    // Internal projection methods. These are used by message consumers to keep the read model up to date.
    Task<Customer?> GetByIdEntityAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Customer?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<Customer?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cancellationToken = default);
    Task<bool> PhoneNumberExistsAsync(string phoneNumber, Guid? excludedCustomerId = null, CancellationToken cancellationToken = default);
    Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
    Task UpsertOrderAsync(CustomerOrder order, CancellationToken cancellationToken = default);
    Task UpsertShipmentAsync(CustomerShipment shipment, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
