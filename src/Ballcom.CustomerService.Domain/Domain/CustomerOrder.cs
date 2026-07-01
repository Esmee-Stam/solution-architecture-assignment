namespace Ballcom.CustomerService.Domain.Domain;

public class CustomerOrder
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public DateTime? PlacedAt { get; set; }
    public DateTime? LastStatusChangedAt { get; set; }

    public List<CustomerOrderItem> Items { get; set; } = [];

    public void Update(
        Guid customerId,
        string status,
        decimal totalAmount,
        string currency,
        string paymentMethod,
        DateTime? placedAt,
        DateTime? lastStatusChangedAt,
        IEnumerable<CustomerOrderItem> items)
    {
        CustomerId = customerId;
        Status = NormalizeRequired(status, nameof(status));
        TotalAmount = totalAmount;
        Currency = NormalizeRequired(currency, nameof(currency));
        PaymentMethod = NormalizeRequired(paymentMethod, nameof(paymentMethod));
        PlacedAt = placedAt ?? PlacedAt;
        LastStatusChangedAt = lastStatusChangedAt ?? LastStatusChangedAt;
        Items = items.ToList();
    }

    private static string NormalizeRequired(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{fieldName} is required.", fieldName);
        }

        return value.Trim();
    }
}
