using Ballcom.Order.Application.Services;
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

                Console.WriteLine(message );

                await service.AddProduct(
                    message.CustomerId,
                    message.ProductId,
                    message.ProductName,
                    message.Quantity,
                    message.Price,
                    message.Currency
                );

                await context.RespondAsync(new AddProductToCartSuccess("Product added successfully."));
            }
            catch (DomainException ex)
            {
                await context.RespondAsync(new AddProductToCartFailed(ex.Message));
            }
        }
    }
}
