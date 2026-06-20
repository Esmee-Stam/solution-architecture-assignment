using Ballcom.ProductCatalog.Application.Interfaces;
using Ballcom.ProductCatalog.Domain.Domain;
using Ballcom.ProductCatalog.Domain.ValueObjects;
using Events.ProductCatalogEvents;
using MassTransit;

namespace Ballcom.ProductCatalog.Application.Commands.CreateProduct
{
    public class CreateProductHandler(IProductWriteRepository repository, IPublishEndpoint publishEndpoint)
    {
        public async Task<Product> Handle(CreateProductCommand command)
        {
            var product = new Product(
                Guid.NewGuid(),
                command.Name,
                command.Description,
                new Money(command.PriceAmount, command.Currency),
                new Stock(command.Stock)
            );

            await repository.AddProductAsync(product);

            await publishEndpoint.Publish(
                new ProductCreatedEvent(
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price.Amount,
                    product.Price.Currency,
                    product.Quantity.Value
                )
            );

            return product;
        }
    }
}
