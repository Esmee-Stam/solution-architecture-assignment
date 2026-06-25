using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Application.Services;
using Ballcom.Order.Domain.Domain;
using Ballcom.Order.Domain.Exceptions;
using Events.ProductCatalogEvents;
using MassTransit;

namespace Ballcom.Order.Infrastructure.Messaging
{
    public class ProductAddedToCartConsumer(
        ShoppingCartService service
    ) : IConsumer<AddProductToCartRequestedEvent>
    {
        public async Task Consume(ConsumeContext<AddProductToCartRequestedEvent> context)
        {
            try
            {
                var message = context.Message;

                await service.AddProduct(message.CustomerId, message.ProductId, message.ProductName, message.Quantity);

                await context.RespondAsync(new AddProductToCartSuccess("Product added successfully."));
            }
            catch (DomainException ex)
            {
                await context.RespondAsync(new AddProductToCartFailed(ex.Message));
            }

            //try
            //{
            //    var message = context.Message;

            //    var cart = await cartWriteRepository.GetOrCreateAsync(message.CustomerId);


            //    cart.AddProduct(
            //        message.ProductId,
            //        message.ProductName,
            //        message.Quantity);

            //    await cartWriteRepository.SaveAsync(cart);

            //    await context.Publish(new CartItemAddedEvent(
            //        message.CustomerId,
            //        message.ProductId,
            //        message.ProductName,
            //        message.Quantity));

            //    await context.RespondAsync(new AddProductToCartSuccess("Product added successfully."));
            //}
            //catch (DomainException ex)
            //{
            //    await context.RespondAsync(new AddProductToCartFailed(ex.Message));
            //}

        }
        

    }
}
