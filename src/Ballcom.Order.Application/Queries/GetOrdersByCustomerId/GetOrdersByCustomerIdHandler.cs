using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Interfaces;
using OrderAggregate = Ballcom.Order.Domain.Domain.Order;

namespace Ballcom.Order.Application.Queries.GetOrdersByCustomerId;

public class GetOrdersByCustomerIdHandler(IOrderReadRepository readRepository)
{
    public async Task<IReadOnlyCollection<OrderDto>> Handle(GetOrdersByCustomerIdQuery query, CancellationToken cancellationToken = default)
    {
        var orders = await readRepository.GetOrderByCustomerIdAsync(query.CustomerId);

        return orders.Select(o => new OrderDto
        {
            Id = o.Id,
            CustomerId = o.CustomerId,
            PaymentMethod = o.PaymentMethod,
            Status = o.Status,
            CreatedAt = o.CreatedAt,
            OrderItems = o.OrderItems.Select(oi => new OrderItemDto
            {
                ProductId = oi.ProductId,
                ProductName = oi.ProductName,
                Quantity = oi.Quantity
            }).ToList()
        }).ToList();
    }
}
