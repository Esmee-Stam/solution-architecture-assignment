namespace Ballcom.Order.Domain.ValueObjects;

public enum OrderStatus
{
    Draft = 0,
    Confirmed = 1,
    Paid = 2,
    Picking = 3,
    Packed = 4,
    Shipped = 5,
    Cancelled = 6
}
