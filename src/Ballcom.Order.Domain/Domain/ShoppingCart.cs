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

    public void AddProduct(Guid productId, string productName, int quantity)
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
                    quantity));
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

    //public Guid Id { get; private set; }
    //public Guid CustomerId { get; private set; }
    //public List<CartItem> CartItems { get; private set; }
    //public DateTime CreatedAt { get; private set; }

    //private ShoppingCart()
    //{
    //    CartItems = [];
    //}

    //public ShoppingCart(Guid id, Guid customerId)
    //{
    //    if (id == Guid.Empty) throw new ArgumentException("Cart id is required.", nameof(id));
    //    if (customerId == Guid.Empty) throw new ArgumentException("Customer id is required.", nameof(customerId));

    //    Id = id;
    //    CustomerId = customerId;
    //    CreatedAt = DateTime.UtcNow;
    //    CartItems = [];
    //}

    //public void AddItem(CartItem item)
    //{
    //    if (item is null) throw new ArgumentNullException(nameof(item));

    //    var existing = CartItems.FirstOrDefault(x => x.ProductId == item.ProductId);

    //    if (existing is null)
    //    {
    //        if (CartItems.Count >= 20) throw new DomainException("A shopping cart can contain a maximum of 20 different items.");

    //        CartItems.Add(item);

    //        return;
    //    }

    //    existing.IncreaseQuantity(item.Quantity);
    //}

    //public void RemoveItem(Guid productId)
    //{
    //    var existing = CartItems.FirstOrDefault(x => x.ProductId == productId);
    //    if (existing != null) CartItems.Remove(existing);
    //}

    //public void UpdateCustomer(Guid customerId)
    //{
    //    if (customerId == Guid.Empty) throw new ArgumentException("Customer id is required.", nameof(customerId));
    //    CustomerId = customerId;
    //}

    //public void AddProduct(Guid productId, string productName, int quantity)
    //{
    //    int currentQuantity = CartItems.Sum(item => item.Quantity);

    //    if (currentQuantity + quantity > 20)
    //    {
    //        throw new DomainException("A shopping cart can contain a maximum of 20 different items.");
    //    }

    //    var existing = CartItems.FirstOrDefault(x => x.ProductId == productId);

    //    if (existing is null)
    //    {
    //        if (CartItems.Count >= 20) throw new DomainException("A shopping cart can contain a maximum of 20 different items.");

    //        CartItems.Add(new CartItem(
    //            Guid.NewGuid(),
    //            Id,
    //            productId,
    //            productName,
    //            quantity
    //        ));
    //        return;
    //    }

    //    existing.IncreaseQuantity(quantity);
    //}


    //public void ReplaceItems(List<CartItem> items)
    //{

    //    foreach (var item in items)
    //    {
    //        if (CartItems.Count >= 20) throw new DomainException("A shopping cart can contain a maximum of 20 different items.");

    //        CartItems.Add(item);
    //    }
    //}
    //public void Clear() => CartItems.Clear();

    //public Order Checkout(Guid orderId, PaymentMethod paymentMethod)
    //{
    //    if (!CartItems.Any())
    //        throw new DomainException("A shopping cart must contain at least one item before checkout.");

    //    var order = new Order(orderId, CustomerId, paymentMethod);

    //    foreach (var item in CartItems)
    //        order.AddItem(item.ToOrderItem());

    //    return order;
    //}
}
