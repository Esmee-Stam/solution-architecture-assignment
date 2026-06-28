namespace Ballcom.Shipment.WebAPI.Models;

public class CreateShipmentModel
{
    public Guid WarehouseOrderId { get; set; }
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public string Carrier { get; set; } = "PostNL";
}
