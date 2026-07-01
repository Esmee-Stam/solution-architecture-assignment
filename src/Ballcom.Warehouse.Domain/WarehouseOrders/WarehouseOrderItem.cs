namespace Ballcom.Warehouse.Domain.WarehouseOrders;

public class WarehouseOrderItem
{
    private WarehouseOrderItem()
    {
        ProductName = string.Empty;
        Currency = string.Empty;
    }

    public WarehouseOrderItem(
        Guid id,
        Guid productId,
        string productName,
        int quantity,
        decimal unitPrice,
        string currency)
    {
        if (id == Guid.Empty) throw new ArgumentException("Item id is required.", nameof(id));
        if (productId == Guid.Empty) throw new ArgumentException("Product id is required.", nameof(productId));
        if (string.IsNullOrWhiteSpace(productName)) throw new ArgumentException("Product name is required.", nameof(productName));
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        if (unitPrice < 0) throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative.");
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency is required.", nameof(currency));

        Id = id;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Currency = currency.ToUpperInvariant();
    }

    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string Currency { get; private set; }
}
