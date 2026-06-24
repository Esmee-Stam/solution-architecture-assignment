using Ballcom.Order.Domain.Exceptions;
using Ballcom.Order.Domain.ValueObjects;

namespace Ballcom.Order.Domain.Domain;

public class ShoppingCart
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public List<CartItem> CartItems { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private ShoppingCart()
    {
        CartItems = [];
    }

    public ShoppingCart(Guid id, Guid customerId)
    {
        if (id == Guid.Empty) throw new ArgumentException("Cart id is required.", nameof(id));
        if (customerId == Guid.Empty) throw new ArgumentException("Customer id is required.", nameof(customerId));

        Id = id;
        CustomerId = customerId;
        CreatedAt = DateTime.UtcNow;
        CartItems = [];
    }

    public void AddItem(CartItem item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));

        var existing = CartItems.FirstOrDefault(x => x.ProductId == item.ProductId);

        if (existing is null)
        {
            if (CartItems.Count >= 20)
                throw new DomainException("A shopping cart can contain a maximum of 20 different items.");

            CartItems.Add(item);
            return;
        }

        existing.IncreaseQuantity(item.Quantity);
    }

    public void RemoveItem(Guid productId)
    {
        var existing = CartItems.FirstOrDefault(x => x.ProductId == productId);
        if (existing is null)
            return;

        CartItems.Remove(existing);
    }

    public void Clear() => CartItems.Clear();

    public Order Checkout(Guid orderId, PaymentMethod paymentMethod)
    {
        if (!CartItems.Any())
            throw new DomainException("A shopping cart must contain at least one item before checkout.");

        var order = new Order(orderId, CustomerId, paymentMethod);

        foreach (var item in CartItems)
            order.AddItem(item.ToOrderItem());

        return order;
    }
}
