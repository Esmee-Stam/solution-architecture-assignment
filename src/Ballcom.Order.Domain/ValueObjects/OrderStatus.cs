namespace Ballcom.Order.Domain.ValueObjects;

public enum OrderStatus
{
    Placed = 0,
    Paid = 2,
    Packed = 4,
    Shipped = 5,
    Cancelled = 6
}
