namespace Ballcom.Warehouse.Infrastructure.Data.Read;

public class WarehouseOrderItemReadModel
{
    public Guid Id { get; set; }
    public Guid WarehouseOrderId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Currency { get; set; } = string.Empty;
}
