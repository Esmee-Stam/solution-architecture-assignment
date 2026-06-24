using Ballcom.Order.Domain.Domain;

namespace Ballcom.Order.Application.DTOs;

public record OrderItemDto(Guid ProductId, string ProductName, int Quantity)
{
    public static OrderItemDto FromDomain(OrderItem item) => new(item.ProductId, item.ProductName, item.Quantity);
}
