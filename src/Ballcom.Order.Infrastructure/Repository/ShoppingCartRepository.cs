using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Domain.Domain;
using Ballcom.Order.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Order.Infrastructure.Repository;

public class ShoppingCartRepository(ShoppingCartDbContext context) : IShoppingCartRepository
{
    public async Task<ShoppingCart?> GetShoppingCartAsync(Guid id)
    {
        return await context.ShoppingCarts
            .Include("CartItems")
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddAsync(ShoppingCart cart)
    {
        await context.ShoppingCarts.AddAsync(cart);
    }

    public async Task DeleteCartAsync(ShoppingCart cart)
    {
        context.ShoppingCarts.Remove(cart);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<ShoppingCart?> GetShoppingCartByCustomerIdAsync(Guid customerId)
    {
        return await context.ShoppingCarts
            .Include("CartItems")
            .FirstOrDefaultAsync(s => s.CustomerId == customerId);
    }
}
