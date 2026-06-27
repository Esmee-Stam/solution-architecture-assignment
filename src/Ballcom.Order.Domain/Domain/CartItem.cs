using Ballcom.Order.Domain.ValueObjects;

namespace Ballcom.Order.Domain.Domain;

public class CartItem
{
    private CartItem()
    {
        ProductName = string.Empty;
        Price = new Money(0, "EUR");
    }

    public CartItem(
        Guid id,
        Guid productId,
        string productName,
        int quantity,
        Money price)
    {
        Id = id;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        Price = price;
    }

    public Guid Id { get; private set; }

    public Guid ProductId { get; private set; }

    public string ProductName { get; private set; }

    public int Quantity { get; private set; }

    public Money Price { get; private set; }

    public Money TotalPrice => Price.Multiply(Quantity);

    public void IncreaseQuantity(int amount)
    {
        Quantity += amount;
    }

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
    }

    
    //public OrderItem ToOrderItem() => new(ProductId, ProductName, Quantity);
}
