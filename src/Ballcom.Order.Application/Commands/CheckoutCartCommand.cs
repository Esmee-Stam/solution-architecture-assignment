namespace Ballcom.Order.Application.Commands;

using Ballcom.Order.Domain.ValueObjects;

public record CheckoutCartCommand(
    Guid CustomerId,
    PaymentMethod PaymentMethod);
