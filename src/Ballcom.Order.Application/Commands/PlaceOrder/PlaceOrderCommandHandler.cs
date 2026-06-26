using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Application.Orders.Commands;
using Ballcom.Order.Domain.ValueObjects;
using Events.OrderEvents;
using MassTransit;

using OrderAggregate = Ballcom.Order.Domain.Domain.Order;

namespace Ballcom.Order.Application.Commands.PlaceOrder
{
    public class PlaceOrderCommandHandler(
        IShoppingCartRepository shoppingCartRepository,
        IOrderWriteRepository writeRepository,
        IPublishEndpoint endpoint
        )
    {
        public async Task<Guid> Handle(PlaceOrderCommand command)
        {
            var cart = await shoppingCartRepository.GetShoppingCartByCustomerIdAsync(command.CustomerId);
            if (cart is null || !cart.CartItems.Any())
                throw new Exception("Shopping cart is empty or does not exist.");

            var totalAmount = cart.TotalCartPrice.Amount;
            var currency = cart.TotalCartPrice.Currency;
            var orderId = Guid.NewGuid();

            var order = new OrderAggregate(orderId, command.CustomerId, command.PaymentMethod);

            foreach (var item in cart.CartItems)
            {
                order.AddItem(
                    item.ProductId,
                    item.ProductName,
                    new Money(item.Price.Amount, item.Price.Currency),
                    item.Quantity
                );
            }

            await writeRepository.AddAsync(order);

            cart.Checkout(orderId, command.PaymentMethod);

            await shoppingCartRepository.SaveChangesAsync();
            await writeRepository.SaveChangesAsync();

            await endpoint.Publish(new OrderPlacedEvent(
                orderId,
                command.CustomerId,
                command.PaymentMethod.ToString(),
                totalAmount,
                currency
            ));

            return orderId;
        }
    }
}
