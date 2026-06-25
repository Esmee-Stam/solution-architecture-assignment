using Ballcom.Order.Domain.Domain;

namespace Ballcom.Order.Application.Interfaces;

public interface IShoppingCartRepository
{
    Task<ShoppingCart?> GetShoppingCartAsync(Guid id);
    Task<ShoppingCart?> GetShoppingCartByCustomerIdAsync(Guid customerId);

    Task AddAsync(ShoppingCart cart);

    Task DeleteCartAsync(ShoppingCart cart);

    Task SaveChangesAsync();
}
