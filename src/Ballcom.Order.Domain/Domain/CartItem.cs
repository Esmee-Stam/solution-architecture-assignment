namespace Ballcom.Order.Domain.Domain;

public class CartItem
{
    private CartItem()
    {
        ProductName = string.Empty;
    }

    public CartItem(
        Guid id,
        Guid productId,
        string productName,
        int quantity)
    {
        Id = id;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
    }

    public Guid Id { get; private set; }

    public Guid ProductId { get; private set; }

    public string ProductName { get; private set; }

    public int Quantity { get; private set; }

    public void IncreaseQuantity(int amount)
    {
        Quantity += amount;
    }

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
    }

    //public Guid Id { get; private set; }
    //public Guid ShoppingCartId { get; private set; }
    //public Guid ProductId { get; private set; }
    //public string ProductName { get; private set; }
    //public int Quantity { get; private set; }

    //private CartItem()
    //{
    //    ProductName = string.Empty;
    //}

    //public CartItem(Guid id, Guid shoppingCartId, Guid productId, string productName, int quantity)
    //{
    //    if (id == Guid.Empty) throw new ArgumentException("Id is required.", nameof(id));
    //    if (shoppingCartId == Guid.Empty) throw new ArgumentException("ShoppingCartId is required.", nameof(shoppingCartId));
    //    if (productId == Guid.Empty) throw new ArgumentException("ProductId is required.", nameof(productId));
    //    if (string.IsNullOrWhiteSpace(productName)) throw new ArgumentException("ProductName is required.", nameof(productName));
    //    if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive.");

    //    Id = id;
    //    ShoppingCartId = shoppingCartId;
    //    ProductId = productId;
    //    ProductName = productName;
    //    Quantity = quantity;
    //}

    //public void IncreaseQuantity(int amount)
    //{
    //    if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
    //    Quantity += amount;
    //}

    //public OrderItem ToOrderItem() => new(ProductId, ProductName, Quantity);
}
