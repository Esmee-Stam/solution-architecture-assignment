using Ballcom.Order.Domain.Exceptions;
using Ballcom.Order.Domain.ValueObjects;

namespace Ballcom.Order.Domain.Domain;

public class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public List<OrderItem> OrderItems { get; private set; }
    public OrderStatus Status { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Order()
    {
        OrderItems = [];
    }

    public Order(Guid id, Guid customerId, PaymentMethod paymentMethod)
    {
        if (id == Guid.Empty) throw new ArgumentException("Order id is required.", nameof(id));
        if (customerId == Guid.Empty) throw new ArgumentException("Customer id is required.", nameof(customerId));

        Id = id;
        CustomerId = customerId;
        PaymentMethod = paymentMethod;
        Status = OrderStatus.Draft;
        CreatedAt = DateTime.UtcNow;
        OrderItems = [];
    }

    private Order(Guid id, Guid customerId, PaymentMethod paymentMethod, OrderStatus status, DateTime createdAt, List<OrderItem> orderItems)
    {
        Id = id;
        CustomerId = customerId;
        PaymentMethod = paymentMethod;
        Status = status;
        CreatedAt = createdAt;
        OrderItems = orderItems;
    }

    public static Order Rehydrate(Guid id, Guid customerId, PaymentMethod paymentMethod, OrderStatus status, DateTime createdAt, IEnumerable<OrderItem> items)
    {
        return new Order(id, customerId, paymentMethod, status, createdAt, items.ToList());
    }

    public void AddItem(OrderItem item)
    {
        EnsureCanEdit();
        if (item is null) throw new ArgumentNullException(nameof(item));

        var existing = OrderItems.FirstOrDefault(x => x.ProductId == item.ProductId);

        if (existing is null)
        {
            if (OrderItems.Count >= 20)
                throw new DomainException("An order can contain a maximum of 20 different items.");

            OrderItems.Add(item);
            return;
        }

        existing.IncreaseQuantity(item.Quantity);
    }

    public void RemoveItem(Guid productId)
    {
        EnsureCanEdit();

        var existing = OrderItems.FirstOrDefault(x => x.ProductId == productId);
        if (existing is null)
            return;

        OrderItems.Remove(existing);
    }

    public void Confirm()
    {
        if (!OrderItems.Any())
            throw new DomainException("An order must contain at least one item.");

        if (Status != OrderStatus.Draft)
            throw new DomainException("Only a draft order can be confirmed.");

        Status = OrderStatus.Confirmed;
    }

    public void MarkPacked()
    {
        if (Status != OrderStatus.Confirmed)
            throw new DomainException("Only a confirmed order can be packed.");

        Status = OrderStatus.Packed;
    }

    public void MarkShipped()
    {
        if (Status != OrderStatus.Packed)
            throw new DomainException("Only a packed order can be shipped.");

        Status = OrderStatus.Shipped;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Shipped)
            throw new DomainException("A shipped order cannot be cancelled.");

        Status = OrderStatus.Cancelled;
    }

    private void EnsureCanEdit()
    {
        if (Status != OrderStatus.Draft)
            throw new DomainException("Only a draft order can be edited.");
    }
}
