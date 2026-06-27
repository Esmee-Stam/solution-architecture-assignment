using Ballcom.Order.Domain.Domain;

namespace Ballcom.Order.Application.DTOs;

public record ShoppingCartDto(
    Guid Id,
    Guid CustomerId,
    IReadOnlyCollection<CartItemDto> CartItems,
    DateTime CreatedAt,
    decimal TotalCartAmount,
    string Currency
)
{
    public static ShoppingCartDto FromDomain(ShoppingCart cart)
        => new(
            cart.Id,
            cart.CustomerId,
            cart.CartItems.Select(CartItemDto.FromDomain).ToList(),
            cart.CreatedAt,
            cart.TotalCartPrice.Amount,
            cart.TotalCartPrice.Currency);
}
