using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Application.Orders.Commands;
using Ballcom.Order.Domain.ValueObjects;
using Events.OrderEvents;
using Events.OrderEvents.Dto;
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
            var cart = await shoppingCartRepository
                .GetShoppingCartByCustomerIdAsync(command.CustomerId);

            if (cart is null || !cart.CartItems.Any())
                throw new Exception("Shopping cart is empty or does not exist.");

            var orderId = Guid.NewGuid();

            var order = new OrderAggregate(orderId, command.CustomerId, command.PaymentMethod);

            var items = cart.CartItems
                .Select(item =>
                {
                    var money = new Money(item.Price.Amount, item.Price.Currency);

                    order.AddItem(
                        item.ProductId,
                        item.ProductName,
                        money,
                        item.Quantity);

                    return new OrderItemEventDto(
                        item.ProductId,
                        item.ProductName,
                        item.Quantity,
                        item.Price.Amount
                    );
                })
                .ToList();

            await writeRepository.AddAsync(order);
            await writeRepository.SaveChangesAsync();

            cart.Checkout(orderId, command.PaymentMethod);

            await shoppingCartRepository.DeleteCartAsync(cart);
            await shoppingCartRepository.SaveChangesAsync();

            var total = order.TotalPrice;

            if (total.Amount <= 0)
                throw new Exception("Order total is invalid. Event will not be published.");

            if (string.IsNullOrWhiteSpace(total.Currency))
                throw new Exception("Order currency is invalid.");

            await endpoint.Publish(new OrderPlacedEvent(
                orderId,
                command.CustomerId,
                command.PaymentMethod.ToString(),
                total.Amount,
                total.Currency,
                items
            ));

            return orderId;
        }
    }
}
