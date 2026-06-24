using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Interfaces;

namespace Ballcom.Order.Application.Commands.RemoveCartItem;

public class RemoveCartItemHandler(IShoppingCartReadRepository readRepository, IShoppingCartWriteRepository writeRepository)
{
    public async Task<ShoppingCartDto> Handle(RemoveCartItemCommand command, CancellationToken cancellationToken = default)
    {
        var cart = await readRepository.GetByCustomerIdAsync(command.CustomerId, cancellationToken)
                   ?? throw new InvalidOperationException("Shopping cart not found.");

        cart.RemoveItem(command.ProductId);
        await writeRepository.SaveAsync(cart, cancellationToken);

        return ShoppingCartDto.FromDomain(cart);
    }
}
