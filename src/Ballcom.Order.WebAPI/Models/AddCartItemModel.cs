namespace Ballcom.Order.WebApi.Models;

public record AddCartItemModel(Guid CustomerId, Guid ProductId, int Quantity);
