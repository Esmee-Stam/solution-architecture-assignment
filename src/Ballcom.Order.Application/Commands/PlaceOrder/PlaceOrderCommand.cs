using Ballcom.Order.Domain.ValueObjects;

namespace Ballcom.Order.Application.Orders.Commands;

public record PlaceOrderCommand(Guid CustomerId, PaymentMethod PaymentMethod);