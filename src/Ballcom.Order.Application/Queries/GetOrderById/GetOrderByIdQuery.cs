namespace Ballcom.Order.Application.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId, Guid CustomerId);
