using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Interfaces;
using OrderAggregate = Ballcom.Order.Domain.Domain.Order;

namespace Ballcom.Order.Application.Queries.GetOrderById;

public class GetOrderByIdHandler(IOrderReadRepository readRepository)
{
    public async Task<OrderDto?> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken = default)
    {
        //OrderAggregate? order = await readRepository.GetByIdAsync(query.OrderId, cancellationToken);
        //return order is null ? null : OrderDto.FromDomain(order);
        return null;
    }
}
