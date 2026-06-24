using ShoppingCartAggregate = Ballcom.Order.Domain.Domain.ShoppingCart;

namespace Ballcom.Order.Application.Interfaces;

public interface IShoppingCartWriteRepository
{
    Task SaveAsync(ShoppingCartAggregate cart, CancellationToken cancellationToken = default);
}
