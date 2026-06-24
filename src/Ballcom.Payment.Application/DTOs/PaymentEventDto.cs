namespace Ballcom.Payment.Application.DTOs;

public class PaymentEventDto
{
    public Guid Id { get; set; }
    public Guid PaymentId { get; set; }

    public string EventType { get; set; } = string.Empty;
    public string EventDataJson { get; set; } = string.Empty;

    public DateTime OccurredAt { get; set; }
    public int Version { get; set; }
}