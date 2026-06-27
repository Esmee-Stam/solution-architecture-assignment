using OrderAggregate = Ballcom.Order.Domain.Domain.Order;

namespace Ballcom.Order.Application.Interfaces;

public interface IOrderWriteRepository
{
    Task AddAsync(OrderAggregate order);
    Task<OrderAggregate?> GetOrderByIdAsync(Guid id);
    Task SaveChangesAsync();

    //TODO: verwijderen
    Task SaveAsync(OrderAggregate order, CancellationToken cancellationToken = default);
}
