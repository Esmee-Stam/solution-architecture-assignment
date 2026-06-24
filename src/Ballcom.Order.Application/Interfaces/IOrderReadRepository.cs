using OrderAggregate = Ballcom.Order.Domain.Domain.Order;

namespace Ballcom.Order.Application.Interfaces;

public interface IOrderReadRepository
{
    Task<OrderAggregate?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<OrderAggregate>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
}
