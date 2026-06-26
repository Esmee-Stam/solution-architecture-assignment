using Ballcom.Order.Domain.Exceptions;
using Ballcom.Order.Domain.ValueObjects;

namespace Ballcom.Order.Domain.Domain;

public class Order
{
    private readonly List<OrderItem> _orderItems = new();

    private Order() { }

    public Order(Guid id, Guid customerId, PaymentMethod paymentMethod)
    {
        if (customerId == Guid.Empty) throw new ArgumentException("CustomerId cannot be empty.", nameof(customerId));

        Id = id;
        CustomerId = customerId;
        Status = OrderStatus.Placed;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }
    public PaymentMethod PaymentMethod { get; set; }

    public OrderStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public byte[] RowVersion { get; private set; }

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public Money TotalPrice
    {
        get
        {
            if (!_orderItems.Any())
                return new Money(0m, "EUR");

            var total = _orderItems
                .Select(x => x.TotalPrice)
                .Aggregate((total, next) => total + next);

            return total;
        }
    }

    public void AddItem(
        Guid productId,
        string productName,
        Money unitPrice,
        int quantity)
    {

        var existing = _orderItems.FirstOrDefault(x => x.ProductId == productId);

        if (existing is null)
        {
            if (_orderItems.Count >= 20) throw new DomainException("An order can contain a maximum of 20 different items.");

            _orderItems.Add(new OrderItem(Guid.NewGuid(), productId, productName, unitPrice, quantity));

            return;
        }

        existing.IncreaseQuantity(quantity);

    }

    public void RemoveItem(Guid productId)
    {
        var item = _orderItems.FirstOrDefault(x => x.ProductId == productId);

        if (item is null) return;

        _orderItems.Remove(item);
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Placed) throw new DomainException("Only a placed order can be marked as paid.");

        Status = OrderStatus.Paid;
    }

    public void MarkPacked()
    {
        if (Status != OrderStatus.Paid) throw new DomainException("Only a confirmed order can be packed.");

        Status = OrderStatus.Packed;
    }

    public void MarkShipped()
    {
        if (Status != OrderStatus.Packed) throw new DomainException("Only a packed order can be shipped.");

        Status = OrderStatus.Shipped;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Shipped) throw new DomainException("A shipped order cannot be cancelled.");

        Status = OrderStatus.Cancelled;
    }
}
