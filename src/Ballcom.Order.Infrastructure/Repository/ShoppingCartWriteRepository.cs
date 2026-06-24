using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Infrastructure.Data.Write;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Order.Infrastructure.Repository;

public class ShoppingCartWriteRepository(ShoppingCartWriteDbContext writeDbContext) : IShoppingCartWriteRepository
{
    public async Task SaveAsync(Ballcom.Order.Domain.Domain.ShoppingCart cart, CancellationToken cancellationToken = default)
    {
        var existing = await writeDbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.Id == cart.Id, cancellationToken);
        if (existing is null)
            await writeDbContext.ShoppingCarts.AddAsync(cart, cancellationToken);

        await writeDbContext.SaveChangesAsync(cancellationToken);
    }
}
