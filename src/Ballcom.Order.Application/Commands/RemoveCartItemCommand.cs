namespace Ballcom.Order.Application.Commands;

public record RemoveCartItemCommand(
    Guid CustomerId,
    Guid ProductId);
