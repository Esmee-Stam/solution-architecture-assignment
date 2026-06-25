using Ballcom.Order.Application.Interfaces;
using Events.OrderEvents.ShoppingCartEvents;
using MassTransit;

namespace Ballcom.Order.Application.Commands.RemoveCartItem;

public class RemoveCartItemHandler(
    IShoppingCartReadRepository readRepository, 
    IShoppingCartWriteRepository writeRepository,
    IPublishEndpoint publishEndpoint
    )
{
    public async Task Handle(RemoveCartItemCommand command)
    {
        var cart = await readRepository.GetByCustomerIdAsync(command.CustomerId);

        if (cart is null) return;

        cart.RemoveItem(command.ProductId);

        await writeRepository.SaveAsync(cart);

        await publishEndpoint.Publish(new CartItemRemovedEvent(
            command.CustomerId,
            command.ProductId
        ));
    }
}
