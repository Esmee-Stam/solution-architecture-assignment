using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Infrastructure.Data.Read;
using Ballcom.Order.Infrastructure.Data.Read.Models;
using Microsoft.EntityFrameworkCore;

using ShoppingCartAggregate = Ballcom.Order.Domain.Domain.ShoppingCart;
using CartItemAggregate = Ballcom.Order.Domain.Domain.CartItem;

namespace Ballcom.Order.Infrastructure.Repository;

public class ShoppingCartReadRepository(
    ShoppingCartReadDbContext readDbContext)
    : IShoppingCartReadRepository
{
    public async Task<ShoppingCartAggregate?> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        var model = await readDbContext.ShoppingCarts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(
                x => x.CustomerId == customerId,
                cancellationToken);

        return model is null ? null : Map(model);
    }

    private static ShoppingCartAggregate Map(ShoppingCartReadModel model)
    {
        var cart = new ShoppingCartAggregate(
            model.Id,
            model.CustomerId);

        foreach (var item in model.CartItems)
        {
            cart.AddItem(new CartItemAggregate(
                item.ProductId,
                item.ProductName,
                item.Quantity));
        }

        return cart;
    }
}