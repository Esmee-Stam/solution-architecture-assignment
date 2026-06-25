using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Domain.Domain;
using Events.OrderEvents.ShoppingCartEvents;
using Events.ProductCatalogEvents;
using MassTransit;

namespace Ballcom.Order.Infrastructure.Messaging
{
    public class ProductAddedToCartConsumer(
        IShoppingCartReadRepository cartReadRepository,
        IShoppingCartWriteRepository cartWriteRepository
        ) : IConsumer<AddProductToCartRequestedEvent>
    {
        public async Task Consume(ConsumeContext<AddProductToCartRequestedEvent> context)
        {
            var message = context.Message;

            var cart = await cartReadRepository.GetByCustomerIdAsync(message.CustomerId) ?? new ShoppingCart(Guid.NewGuid(), message.CustomerId);

            cart.AddProduct(message.ProductId, message.ProductName, message.Quantity);

            await cartWriteRepository.SaveAsync(cart);

            await context.Publish(new CartItemAddedEvent(
                    message.CustomerId,
                    message.ProductId,
                    message.ProductName,
                    message.Quantity
                ));
        }

    }
}
