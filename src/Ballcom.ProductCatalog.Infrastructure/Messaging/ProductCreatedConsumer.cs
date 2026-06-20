using Ballcom.ProductCatalog.Infrastructure.Data.Read;
using Events.ProductCatalogEvents;
using MassTransit;

namespace Ballcom.ProductCatalog.Infrastructure.Messaging
{
    public class ProductCreatedConsumer(ProductCatalogReadDbContext DbContext) : IConsumer<ProductCreatedEvent>
    {
        public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
        {
            var message = context.Message;

            var product = new ProductReadModel
            {
                Id = message.Id,
                Name = message.Name,
                Description = message.Description,
                PriceAmount = message.PriceAmount,
                Currency = message.Currency,
                Stock = message.Stock
            };

            DbContext.Products.Add(product);

            await DbContext.SaveChangesAsync();
        }
    }
}
