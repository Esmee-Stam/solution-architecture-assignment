using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.IntegrationEvents;
using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Domain.Domain;

namespace Ballcom.Order.Application.Commands.CheckoutCart;

public class CheckoutCartHandler(
    IShoppingCartRepository cartWriteRepository,
    IOrderWriteRepository orderWriteRepository
    )
{
    public async Task<OrderDto> Handle(CheckoutCartCommand command, CancellationToken cancellationToken = default)
    {
        //var cart = await cartReadRepository.GetShoppingCartByCustomerIdAsync(command.CustomerId)
        //           ?? throw new InvalidOperationException("Shopping cart not found.");

        //var order = cart.Checkout(Guid.NewGuid(), command.PaymentMethod);

        //await orderWriteRepository.SaveAsync(order, cancellationToken);

        //cart.Clear();
        //await cartWriteRepository.SaveAsync(cart);

        //await eventPublisher.PublishAsync(
        //    new OrderPlacedIntegrationEvent(
        //        order.Id,
        //        order.CustomerId,
        //        order.OrderItems.Select(x => new OrderPlacedItemDto(x.ProductId, x.ProductName, x.Quantity)).ToList()),
        //    cancellationToken);

        //return OrderDto.FromDomain(order);

        throw new NotImplementedException();
    }
}
