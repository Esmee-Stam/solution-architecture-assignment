using ShoppingCartAggregate = Ballcom.Order.Domain.Domain.ShoppingCart;

namespace Ballcom.Order.Application.Interfaces;

public interface IShoppingCartReadRepository
{
    Task<ShoppingCartAggregate?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
}
