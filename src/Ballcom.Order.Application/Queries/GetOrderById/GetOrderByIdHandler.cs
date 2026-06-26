using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Interfaces;
using OrderAggregate = Ballcom.Order.Domain.Domain.Order;

namespace Ballcom.Order.Application.Queries.GetOrderById;

public class GetOrderByIdHandler(IOrderReadRepository readRepository)
{
    public async Task<OrderDto?> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken = default)
    {
        var order = await readRepository.GetByIdAsync(query.OrderId);

        if (order == null) return null;

        if (order.CustomerId != query.CustomerId) return null;

        return new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            PaymentMethod = order.PaymentMethod,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                ProductId = oi.ProductId,
                ProductName = oi.ProductName,
                Quantity = oi.Quantity
            }).ToList()
        };
    }
}
