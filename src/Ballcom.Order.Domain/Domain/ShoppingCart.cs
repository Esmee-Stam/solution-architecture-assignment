using Ballcom.Order.Domain.Exceptions;
using Ballcom.Order.Domain.ValueObjects;

namespace Ballcom.Order.Domain.Domain;

public class ShoppingCart
{
    private readonly List<CartItem> _cartItems = new();
    private ShoppingCart() {}

    public ShoppingCart(Guid customerId)
    {
        if (customerId == Guid.Empty) throw new ArgumentException(nameof(customerId));

        CustomerId = customerId;
        Id = Guid.NewGuid(); 
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }

    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

    public byte[] RowVersion { get; set; }

    public Money TotalCartPrice
    {
        get
        {
            var currency = _cartItems.FirstOrDefault()?.Price.Currency ?? "EUR";
            return _cartItems.Aggregate(new Money(0, currency), (current, item) => current + item.TotalPrice);
        }
    }

    public void AddProduct(Guid productId, string productName, int quantity, decimal amount, string currency)
    {
        if (quantity <= 0) throw new DomainException("Quantity must be greater than zero.");

        int currentTotalQuantity = _cartItems.Sum(item => item.Quantity);

        if (currentTotalQuantity + quantity > 20)
        {
            throw new DomainException("A shopping cart can contain a maximum of 20 total items.");
        }

        var existing = _cartItems.FirstOrDefault(x => x.ProductId == productId);

        if (existing is null)
        {
            if (_cartItems.Count >= 20)
                throw new DomainException("Maximum 20 different products.");

            _cartItems.Add(
                new CartItem(
                    Guid.NewGuid(),
                    productId,
                    productName,
                    quantity,
                    new Money(amount, currency)));
            return;
        }

        existing.IncreaseQuantity(quantity);
    }

    public void RemoveProduct(Guid productId)
    {
        var item = _cartItems.FirstOrDefault(x => x.ProductId == productId);

        if (item is not null)
            _cartItems.Remove(item);
    }

    public void Clear()
    {
        _cartItems.Clear();
    }

    public Order Checkout(Guid orderId, PaymentMethod paymentMethod)
    {
        if (!_cartItems.Any()) throw new DomainException("A shopping cart must contain at least one item before checkout.");

        var order = new Order(orderId, CustomerId, paymentMethod);

        foreach(var item in _cartItems)
        {
            order.RemoveItem(item.ProductId); 
        }

        Clear();

        return order;
    }
}
