namespace Ballcom.Order.Application.Commands.AddCartItem;

public record AddCartItemCommand(Guid CustomerId, Guid ProductId, int Quantity);
