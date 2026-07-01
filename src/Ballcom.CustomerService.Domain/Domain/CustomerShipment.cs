namespace Ballcom.CustomerService.Domain.Domain;

public class CustomerShipment
{
    public Guid ShipmentId { get; set; }
    public Guid WarehouseOrderId { get; set; }
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Carrier { get; set; } = string.Empty;
    public string TrackingNumber { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? DispatchedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }

    public void Update(
        Guid warehouseOrderId,
        Guid orderId,
        Guid customerId,
        string status,
        string carrier,
        string trackingNumber,
        DateTime occurredAt)
    {
        WarehouseOrderId = warehouseOrderId == Guid.Empty ? WarehouseOrderId : warehouseOrderId;
        OrderId = orderId;
        CustomerId = customerId;
        Status = NormalizeRequired(status, nameof(status));
        Carrier = NormalizeRequired(carrier, nameof(carrier));
        TrackingNumber = NormalizeRequired(trackingNumber, nameof(trackingNumber));
        LastUpdatedAt = occurredAt;

        if (Status.Equals("Created", StringComparison.OrdinalIgnoreCase))
        {
            CreatedAt ??= occurredAt;
        }
        else if (Status.Equals("Dispatched", StringComparison.OrdinalIgnoreCase))
        {
            DispatchedAt ??= occurredAt;
        }
        else if (Status.Equals("Delivered", StringComparison.OrdinalIgnoreCase))
        {
            DeliveredAt ??= occurredAt;
        }
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
