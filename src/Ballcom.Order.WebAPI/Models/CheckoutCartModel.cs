using Ballcom.Order.Domain.ValueObjects;

namespace Ballcom.Order.WebApi.Models;

public record CheckoutCartModel(Guid CustomerId, PaymentMethod PaymentMethod);
