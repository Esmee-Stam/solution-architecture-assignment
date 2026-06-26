using Ballcom.Order.Domain.ValueObjects;

namespace Ballcom.Order.Domain.Domain;

public class OrderItem
{
    private OrderItem()
    {
        ProductName = string.Empty;
        UnitPrice = default;
    }

    public OrderItem(
        Guid id,
        Guid productId,
        string productName,
        Money unitPrice,
        int quantity
    )
    {
        if (id == Guid.Empty) throw new ArgumentException("Id is required.", nameof(id));

        if (productId == Guid.Empty) throw new ArgumentException("Product id is required.", nameof(productId));

        if (string.IsNullOrWhiteSpace(productName)) throw new ArgumentException("Product name is required.", nameof(productName));

        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive.");

        Id = id;
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public Guid Id { get; private set; }

    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public Money TotalPrice => UnitPrice.Multiply(Quantity);

    public void IncreaseQuantity(int amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));

        Quantity += amount;
    }
}