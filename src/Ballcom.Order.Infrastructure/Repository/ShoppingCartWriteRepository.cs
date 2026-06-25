using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Domain.Domain;
using Ballcom.Order.Infrastructure.Data.Write;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Order.Infrastructure.Repository;

public class ShoppingCartWriteRepository(ShoppingCartWriteDbContext writeDbContext) : IShoppingCartWriteRepository
{
    public async Task SaveAsync(ShoppingCart cart)
    {
        var existing = await writeDbContext.ShoppingCarts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.Id == cart.Id);

        if (existing is null)
        {
            await writeDbContext.ShoppingCarts.AddAsync(cart);
        }
        else
        {
            existing.UpdateCustomer(existing.CustomerId);

            writeDbContext.CartItems.RemoveRange(existing.CartItems);

            existing.ReplaceItems(cart.CartItems);
        }

        await writeDbContext.SaveChangesAsync();
    }
}
