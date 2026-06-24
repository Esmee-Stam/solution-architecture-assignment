namespace Ballcom.Order.Domain.Domain;

public class CartItem
{
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }

    private CartItem()
    {
        ProductName = string.Empty;
    }

    public CartItem(Guid productId, string productName, int quantity)
    {
        if (productId == Guid.Empty) throw new ArgumentException("Product id is required.", nameof(productId));
        if (string.IsNullOrWhiteSpace(productName)) throw new ArgumentException("Product name is required.", nameof(productName));
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive.");

        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
    }

    public void IncreaseQuantity(int amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
        Quantity += amount;
    }

    public OrderItem ToOrderItem() => new(ProductId, ProductName, Quantity);
}
