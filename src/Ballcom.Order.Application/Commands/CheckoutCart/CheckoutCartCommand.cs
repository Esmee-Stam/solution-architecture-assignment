using Ballcom.Order.Domain.ValueObjects;

namespace Ballcom.Order.Application.Commands.CheckoutCart;

public record CheckoutCartCommand(Guid CustomerId, PaymentMethod PaymentMethod);
