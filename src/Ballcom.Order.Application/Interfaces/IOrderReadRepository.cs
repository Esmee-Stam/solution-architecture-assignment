using Ballcom.Order.Application.DTOs;
using OrderAggregate = Ballcom.Order.Domain.Domain.Order;

namespace Ballcom.Order.Application.Interfaces;

public interface IOrderReadRepository
{
    Task<OrderDto?> GetByIdAsync(Guid orderId);
    Task<IEnumerable<OrderDto>> GetOrderByCustomerIdAsync(Guid customerId);
  
    Task<IReadOnlyCollection<OrderAggregate>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
}
