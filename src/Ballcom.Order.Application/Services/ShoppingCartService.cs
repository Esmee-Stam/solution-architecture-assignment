using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Domain.Domain;
using Ballcom.Order.Domain.ValueObjects;
using Events.OrderEvents;
using MassTransit;
using MassTransit.Transports;

namespace Ballcom.Order.Application.Services
{
    public class ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IPublishEndpoint endpoint)
    {
        public async Task<Guid> CreateCart(Guid customerId)
        {
            var cart = new ShoppingCart(customerId);

            await shoppingCartRepository.AddAsync(cart);

            await shoppingCartRepository.SaveChangesAsync();

            return cart.Id;
        }

        public async Task AddProduct(
            Guid customerId,
            Guid productId,
            string productName,
            int quantity,
            decimal amount,
            string currency
            )
        {
            var cart = await shoppingCartRepository.GetShoppingCartByCustomerIdAsync(customerId);

            if (cart is null)
            {
                cart = new ShoppingCart(customerId);

                await shoppingCartRepository.AddAsync(cart);
            }

            cart.AddProduct(
                productId,
                productName,
                quantity,
                amount,
                currency);

            await shoppingCartRepository.SaveChangesAsync();
        }

        public async Task RemoveProduct(Guid cartId, Guid productId)
        {
            var cart = await shoppingCartRepository.GetShoppingCartAsync(cartId);

            if (cart is null) throw new Exception("Cart not found");

            cart.RemoveProduct(productId);

            await shoppingCartRepository.SaveChangesAsync();
        }

        public async Task DeleteCart(Guid cartId)
        {
            var cart = await shoppingCartRepository.GetShoppingCartAsync(cartId);
            if (cart is null) return;

            await shoppingCartRepository.DeleteCartAsync(cart);

            await shoppingCartRepository.SaveChangesAsync();
        }

        public async Task<ShoppingCart?> GetCart(Guid cartId)
        {
            return await shoppingCartRepository.GetShoppingCartByCustomerIdAsync(cartId);
        }

        public async Task<ShoppingCartDto?> GetByCustomerId(Guid customerId)
        {
            var cart = await shoppingCartRepository.GetShoppingCartByCustomerIdAsync(customerId);
            return cart is null ? null : ShoppingCartDto.FromDomain(cart);
        }

        public async Task<Guid> CheckoutAsync(Guid customerId, PaymentMethod paymentMethod)
        {
            var cart = await shoppingCartRepository.GetShoppingCartByCustomerIdAsync(customerId);

            if (cart is null || !cart.CartItems.Any()) throw new Exception("Shopping cart is empty or does not exist");

            var totalAmount = cart.TotalCartPrice.Amount;
            var currency = cart.TotalCartPrice.Currency;

            var orderId = Guid.NewGuid();

            //// add order here
            ///

            _ = cart.Checkout(orderId, paymentMethod);

            await shoppingCartRepository.SaveChangesAsync();

            await endpoint.Publish(new OrderPlacedEvent(
                orderId,
                customerId,
                paymentMethod.ToString(), 
                totalAmount,
                currency
            ));

            return orderId;
        }
    }
}
