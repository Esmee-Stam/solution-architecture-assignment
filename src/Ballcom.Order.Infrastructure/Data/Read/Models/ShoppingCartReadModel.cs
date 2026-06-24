namespace Ballcom.Order.Infrastructure.Data.Read.Models;

public class ShoppingCartReadModel
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CartItemReadModel> CartItems { get; set; } = [];
}
