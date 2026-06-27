using Ballcom.Order.Application.Interfaces;
using MassTransit;

namespace Ballcom.Order.Application.Commands.RemoveCartItem;

public class RemoveCartItemHandler(
    IShoppingCartRepository writeRepository,
    IPublishEndpoint publishEndpoint
    )
{
    public async Task Handle(RemoveCartItemCommand command)
    {
        throw new NotImplementedException();

        //var cart = await writeRepository.GetOrCreateAsync(command.CustomerId);

        //if (cart is null) return;

        //cart.RemoveItem(command.ProductId);

        //await writeRepository.SaveAsync(cart);

        //await publishEndpoint.Publish(new CartItemRemovedEvent(
        //    command.CustomerId,
        //    command.ProductId
        //));
    }
}
