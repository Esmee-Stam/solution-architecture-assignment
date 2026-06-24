using Ballcom.Order.Domain.Domain;

namespace Ballcom.Order.Application.DTOs;

public record CartItemDto(Guid ProductId, string ProductName, int Quantity)
{
    public static CartItemDto FromDomain(CartItem item) => new(item.ProductId, item.ProductName, item.Quantity);
}
