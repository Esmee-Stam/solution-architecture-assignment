namespace Ballcom.Warehouse.Domain.WarehouseOrders;

public class WarehouseOrder
{
    private readonly List<WarehouseOrderItem> _items = new();

    private WarehouseOrder()
    {
        Currency = string.Empty;
        PaymentMethod = string.Empty;
    }

    public WarehouseOrder(
        Guid id,
        Guid orderId,
        Guid customerId,
        decimal totalAmount,
        string currency,
        string paymentMethod,
        IEnumerable<WarehouseOrderItem> items)
    {
        if (id == Guid.Empty) throw new ArgumentException("Warehouse order id is required.", nameof(id));
        if (orderId == Guid.Empty) throw new ArgumentException("Order id is required.", nameof(orderId));
        if (customerId == Guid.Empty) throw new ArgumentException("Customer id is required.", nameof(customerId));
        if (totalAmount < 0) throw new ArgumentOutOfRangeException(nameof(totalAmount), "Total amount cannot be negative.");
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency is required.", nameof(currency));
        if (string.IsNullOrWhiteSpace(paymentMethod)) throw new ArgumentException("Payment method is required.", nameof(paymentMethod));

        var itemList = items.ToList();
        if (!itemList.Any()) throw new InvalidOperationException("A warehouse order must contain at least one item.");

        Id = id;
        OrderId = orderId;
        CustomerId = customerId;
        TotalAmount = totalAmount;
        Currency = currency.ToUpperInvariant();
        PaymentMethod = paymentMethod;
        Status = WarehouseOrderStatus.PickRequested;
        CreatedAt = DateTime.UtcNow;
        _items.AddRange(itemList);
    }

    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid CustomerId { get; private set; }
    public WarehouseOrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Currency { get; private set; }
    public string PaymentMethod { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? PickedAt { get; private set; }
    public DateTime? PackedAt { get; private set; }

    public IReadOnlyCollection<WarehouseOrderItem> Items => _items.AsReadOnly();

    public void PickItems()
    {
        if (Status != WarehouseOrderStatus.PickRequested)
        {
            throw new InvalidOperationException("Only a pick requested warehouse order can be picked.");
        }

        Status = WarehouseOrderStatus.Picked;
        PickedAt = DateTime.UtcNow;
    }

    public void Pack()
    {
        if (Status != WarehouseOrderStatus.Picked)
        {
            throw new InvalidOperationException("Only a picked warehouse order can be packed.");
        }

        Status = WarehouseOrderStatus.Packed;
        PackedAt = DateTime.UtcNow;
    }
}
