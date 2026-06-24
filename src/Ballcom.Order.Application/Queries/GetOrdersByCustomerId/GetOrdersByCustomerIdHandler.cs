using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Interfaces;
using OrderAggregate = Ballcom.Order.Domain.Domain.Order;

namespace Ballcom.Order.Application.Queries.GetOrdersByCustomerId;

public class GetOrdersByCustomerIdHandler(IOrderReadRepository readRepository)
{
    public async Task<IReadOnlyCollection<OrderDto>> Handle(GetOrdersByCustomerIdQuery query, CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<OrderAggregate> orders = await readRepository.GetByCustomerIdAsync(query.CustomerId, cancellationToken);
        return orders.Select(OrderDto.FromDomain).ToList();
    }
}
