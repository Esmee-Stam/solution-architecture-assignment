using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Domain.Domain;

namespace Ballcom.Order.Application.Commands.AddCartItem;

public class AddCartItemHandler(
    IShoppingCartReadRepository readRepository,
    IShoppingCartWriteRepository writeRepository,
    IProductCatalogClient productCatalogClient)
{
    public async Task<ShoppingCartDto> Handle(AddCartItemCommand command, CancellationToken cancellationToken = default)
    {
        var product = await productCatalogClient.GetByIdAsync(command.ProductId, cancellationToken)
                      ?? throw new InvalidOperationException("Product not found.");

        var cart = await readRepository.GetByCustomerIdAsync(command.CustomerId, cancellationToken)
                   ?? new ShoppingCart(Guid.NewGuid(), command.CustomerId);

        cart.AddItem(new CartItem(product.Id, product.Name, command.Quantity));

        await writeRepository.SaveAsync(cart, cancellationToken);

        return ShoppingCartDto.FromDomain(cart);
    }
}
