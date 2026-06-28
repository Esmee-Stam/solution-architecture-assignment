namespace Ballcom.Warehouse.WebAPI.Models;

public class CreateWarehouseOrderModel
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public string PaymentMethod { get; set; } = "ForwardPay";
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "EUR";
    public List<CreateWarehouseOrderItemModel> Items { get; set; } = new();
}

public class CreateWarehouseOrderItemModel
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Currency { get; set; } = "EUR";
}
