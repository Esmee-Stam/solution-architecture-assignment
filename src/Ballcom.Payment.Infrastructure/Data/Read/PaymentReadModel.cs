namespace Ballcom.Payment.Infrastructure.Data.Read;

public class PaymentReadModel
{
    public Guid PaymentId { get; set; }
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }

    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;

    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public string? FailureReason { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
}