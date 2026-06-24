using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Domain.Domain;
using Ballcom.Order.Infrastructure.Data.Read;
using Ballcom.Order.Infrastructure.Data.Read.Models;
using Microsoft.EntityFrameworkCore;
using OrderAggregate = Ballcom.Order.Domain.Domain.Order;
using OrderItemAggregate = Ballcom.Order.Domain.Domain.OrderItem;

namespace Ballcom.Order.Infrastructure.Repository;

public class OrderReadRepository(OrderReadDbContext readDbContext) : IOrderReadRepository
{
    public async Task<OrderAggregate?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var model = await readDbContext.Orders
            .Include(x => x.OrderItems)
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

        return model is null ? null : Map(model);
    }

    public async Task<IReadOnlyCollection<OrderAggregate>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var models = await readDbContext.Orders
            .Include(x => x.OrderItems)
            .Where(x => x.CustomerId == customerId)
            .ToListAsync(cancellationToken);

        return models.Select(Map).ToList();
    }

    private static OrderAggregate Map(OrderReadModel model)
    {
        var items = model.OrderItems
            .Select(x => new OrderItemAggregate(x.ProductId, x.ProductName, x.Quantity))
            .ToList();

        return OrderAggregate.Rehydrate(
            model.Id,
            model.CustomerId,
            model.PaymentMethod,
            model.Status,
            model.CreatedAt,
            items);
    }
}