namespace Ballcom.Order.Infrastructure.Data.Read
{
    public class OrderItemReadModel
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
        public string Currency { get; set; } = "EUR";

        public int Quantity { get; set; }

        public OrderReadModel? Order { get; set; }
    }
}
