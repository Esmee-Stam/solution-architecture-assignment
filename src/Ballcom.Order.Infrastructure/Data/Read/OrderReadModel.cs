namespace Ballcom.Order.Infrastructure.Data.Read
{
    public class OrderReadModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "EUR";
        public DateTime CreatedAt { get; set; }

        public List<OrderItemReadModel> OrderItems { get; set; } = new();
    }
}
