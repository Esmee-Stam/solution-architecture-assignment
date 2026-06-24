using Ballcom.Order.Domain.Domain;
using Ballcom.Order.Domain.ValueObjects;
using OrderAggregate = Ballcom.Order.Domain.Domain.Order;

namespace Ballcom.Order.Application.DTOs;

public record OrderDto(Guid Id, Guid CustomerId, OrderStatus Status, PaymentMethod PaymentMethod, IReadOnlyCollection<OrderItemDto> OrderItems, DateTime CreatedAt)
{
    public static OrderDto FromDomain(OrderAggregate order)
        => new(order.Id, order.CustomerId, order.Status, order.PaymentMethod, order.OrderItems.Select(OrderItemDto.FromDomain).ToList(), order.CreatedAt);
}
