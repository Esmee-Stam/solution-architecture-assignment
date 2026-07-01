namespace Ballcom.Shipment.Domain.Shipments;

public class Shipment
{
    private Shipment()
    {
        Carrier = string.Empty;
        TrackingNumber = string.Empty;
    }

    public Shipment(
        Guid id,
        Guid warehouseOrderId,
        Guid orderId,
        Guid customerId,
        string carrier,
        string? trackingNumber = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Shipment id is required.", nameof(id));
        if (warehouseOrderId == Guid.Empty) throw new ArgumentException("Warehouse order id is required.", nameof(warehouseOrderId));
        if (orderId == Guid.Empty) throw new ArgumentException("Order id is required.", nameof(orderId));
        if (customerId == Guid.Empty) throw new ArgumentException("Customer id is required.", nameof(customerId));
        if (string.IsNullOrWhiteSpace(carrier)) throw new ArgumentException("Carrier is required.", nameof(carrier));

        Id = id;
        WarehouseOrderId = warehouseOrderId;
        OrderId = orderId;
        CustomerId = customerId;
        Carrier = carrier.Trim();
        TrackingNumber = string.IsNullOrWhiteSpace(trackingNumber)
            ? GenerateTrackingNumber()
            : trackingNumber.Trim().ToUpperInvariant();
        Status = ShipmentStatus.ShipmentCreated;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid WarehouseOrderId { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid CustomerId { get; private set; }
    public ShipmentStatus Status { get; private set; }
    public string Carrier { get; private set; }
    public string TrackingNumber { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DispatchedAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }

    public void Dispatch()
    {
        if (Status != ShipmentStatus.ShipmentCreated)
        {
            throw new InvalidOperationException("Only a created shipment can be dispatched.");
        }

        Status = ShipmentStatus.Dispatched;
        DispatchedAt = DateTime.UtcNow;
    }

    public void Deliver()
    {
        if (Status != ShipmentStatus.Dispatched)
        {
            throw new InvalidOperationException("Only a dispatched shipment can be delivered.");
        }

        Status = ShipmentStatus.Delivered;
        DeliveredAt = DateTime.UtcNow;
    }

    private static string GenerateTrackingNumber()
    {
        return $"BC-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"[..32].ToUpperInvariant();
    }
}
