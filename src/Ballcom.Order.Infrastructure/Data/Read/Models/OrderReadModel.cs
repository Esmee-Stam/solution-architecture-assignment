using Ballcom.Order.Domain.ValueObjects;

namespace Ballcom.Order.Infrastructure.Data.Read.Models;

public class OrderReadModel
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItemReadModel> OrderItems { get; set; } = [];
}
