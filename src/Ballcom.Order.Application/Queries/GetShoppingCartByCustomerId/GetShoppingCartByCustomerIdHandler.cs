using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Interfaces;

namespace Ballcom.Order.Application.Queries.GetShoppingCartByCustomerId;

public class GetShoppingCartByCustomerIdHandler(IShoppingCartReadRepository readRepository)
{
    public async Task<ShoppingCartDto?> Handle(GetShoppingCartByCustomerIdQuery query, CancellationToken cancellationToken = default)
    {
        var cart = await readRepository.GetByCustomerIdAsync(query.CustomerId, cancellationToken);
        return cart is null ? null : ShoppingCartDto.FromDomain(cart);
    }
}
