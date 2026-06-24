namespace Ballcom.Order.Application.Interfaces;

using Ballcom.Order.Domain.Domain;

public interface IShoppingCartRepository
{
    Task<ShoppingCart?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task SaveAsync(ShoppingCart cart, CancellationToken cancellationToken = default);
}
