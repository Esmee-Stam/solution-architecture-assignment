using OrderAggregate = Ballcom.Order.Domain.Domain.Order;

namespace Ballcom.Order.Application.Interfaces;

public interface IOrderWriteRepository
{
    Task SaveAsync(OrderAggregate order, CancellationToken cancellationToken = default);
}
