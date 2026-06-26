using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Infrastructure.Data.Read;
using Microsoft.EntityFrameworkCore;
using OrderAggregate = Ballcom.Order.Domain.Domain.Order;

namespace Ballcom.Order.Infrastructure.Repository;

public class OrderReadRepository(OrderReadDbContext context) : IOrderReadRepository
{
    public async Task<OrderDto?> GetByIdAsync(Guid orderId)
    {
        return await context.Orders
            .Include(x => x.OrderItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == orderId);
    }

    public async Task<IEnumerable<OrderDto>> GetOrderByCustomerIdAsync(Guid customerId)
    {
        var orders = await context.Orders.Include(x => x.OrderItems)
            .Where(x => x.CustomerId == customerId)
            .AsNoTracking()
            .ToListAsync();

        return orders;
    }

   
    public async Task<IReadOnlyCollection<OrderAggregate>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private static OrderAggregate Map(OrderDto model)
    {
        throw new NotImplementedException();
        //var items = model.OrderItems
        //    .Select(x => new OrderItemAggregate(x.ProductId, x.ProductName, x.Quantity))
        //    .ToList();

        //return OrderAggregate.Rehydrate(
        //    model.Id,
        //    model.CustomerId,
        //    model.PaymentMethod,
        //    model.Status,
        //    model.CreatedAt,
        //    items);
    }
}