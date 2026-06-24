namespace Ballcom.Order.Domain.ValueObjects;

public enum OrderStatus
{
    Draft = 0,
    Confirmed = 1,
    Packed = 2,
    Shipped = 3,
    Cancelled = 4
}
