namespace Ballcom.Order.Application.Commands;

using Ballcom.Order.Domain.ValueObjects;

public record AddCartItemCommand(
    Guid CustomerId,
    Guid ProductId,
    string ProductName,
    int Quantity,
    Money UnitPrice);
