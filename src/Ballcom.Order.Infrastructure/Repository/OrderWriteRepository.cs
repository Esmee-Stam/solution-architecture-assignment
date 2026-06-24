using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Infrastructure.Data.Write;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Order.Infrastructure.Repository;

public class OrderWriteRepository(OrderWriteDbContext writeDbContext) : IOrderWriteRepository
{
    public async Task SaveAsync(Ballcom.Order.Domain.Domain.Order order, CancellationToken cancellationToken = default)
    {
        var existing = await writeDbContext.Orders.FirstOrDefaultAsync(x => x.Id == order.Id, cancellationToken);
        if (existing is null)
            await writeDbContext.Orders.AddAsync(order, cancellationToken);

        await writeDbContext.SaveChangesAsync(cancellationToken);
    }
}
