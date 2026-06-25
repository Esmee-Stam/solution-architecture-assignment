using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Domain.Domain;

namespace Ballcom.Order.Application.Services
{
    public class ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
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
            int quantity
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
                quantity);

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

        public async Task<ShoppingCart?> GetByCustomerId(Guid customerId)
        {
            return await shoppingCartRepository.GetShoppingCartByCustomerIdAsync(customerId);
        }
    }
}
