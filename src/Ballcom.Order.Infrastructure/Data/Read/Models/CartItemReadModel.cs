namespace Ballcom.Order.Infrastructure.Data.Read.Models;

public class CartItemReadModel
{
    public Guid Id { get; set; }
    public Guid ShoppingCartId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
