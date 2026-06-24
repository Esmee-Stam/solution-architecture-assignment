using Ballcom.Payment.Domain.ValueObjects;

namespace Ballcom.Payment.Domain.Payments;

public class Payment
{
    public Guid PaymentId { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid CustomerId { get; private set; }

    public Money Amount { get; private set; }
    public PaymentMethod Method { get; private set; }
    public PaymentStatus Status { get; private set; }

    public string? FailureReason { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime LastUpdatedAt { get; private set; }

    private Payment()
    {
        Amount = null!;
    }

    public Payment(
        Guid paymentId,
        Guid orderId,
        Guid customerId,
        Money amount,
        PaymentMethod method)
    {
        if (paymentId == Guid.Empty)
        {
            throw new ArgumentException("PaymentId is required.");
        }

        if (orderId == Guid.Empty)
        {
            throw new ArgumentException("OrderId is required.");
        }

        if (customerId == Guid.Empty)
        {
            throw new ArgumentException("CustomerId is required.");
        }

        PaymentId = paymentId;
        OrderId = orderId;
        CustomerId = customerId;
        Amount = amount ?? throw new ArgumentNullException(nameof(amount));
        Method = method;
        Status = PaymentStatus.Requested;
        CreatedAt = DateTime.UtcNow;
        LastUpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status == PaymentStatus.Completed)
        {
            throw new InvalidOperationException("Payment is already completed.");
        }

        if (Status == PaymentStatus.Failed)
        {
            throw new InvalidOperationException("Failed payment cannot be completed.");
        }

        Status = PaymentStatus.Completed;
        FailureReason = null;
        LastUpdatedAt = DateTime.UtcNow;
    }

    public void Fail(string reason)
    {
        if (Status == PaymentStatus.Completed)
        {
            throw new InvalidOperationException("Completed payment cannot be failed.");
        }

        if (Status == PaymentStatus.Failed)
        {
            throw new InvalidOperationException("Payment is already failed.");
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Failure reason is required.");
        }

        Status = PaymentStatus.Failed;
        FailureReason = reason;
        LastUpdatedAt = DateTime.UtcNow;
    }
}