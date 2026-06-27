using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Infrastructure.Data.Write;
using Microsoft.EntityFrameworkCore;
using OrderAggregate = Ballcom.Order.Domain.Domain.Order;
namespace Ballcom.Order.Infrastructure.Repository;

public class OrderWriteRepository(OrderWriteDbContext writeDbContext) : IOrderWriteRepository
{
    public async Task AddAsync(OrderAggregate order)
    {
        await writeDbContext.AddAsync(order);
    }

    public async Task<OrderAggregate?> GetOrderByIdAsync(Guid id)
    {
        return await writeDbContext.Orders.Include(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await writeDbContext.SaveChangesAsync();
    }

    public async Task SaveAsync(OrderAggregate order, CancellationToken cancellationToken = default)
    {
        var existing = await writeDbContext.Orders.FirstOrDefaultAsync(x => x.Id == order.Id, cancellationToken);
        if (existing is null)
            await writeDbContext.Orders.AddAsync(order, cancellationToken);

        await writeDbContext.SaveChangesAsync(cancellationToken);
    }
}
