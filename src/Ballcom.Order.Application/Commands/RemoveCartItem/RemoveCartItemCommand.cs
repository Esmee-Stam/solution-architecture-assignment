namespace Ballcom.Order.Application.Commands.RemoveCartItem;

public record RemoveCartItemCommand(Guid CustomerId, Guid ProductId);
