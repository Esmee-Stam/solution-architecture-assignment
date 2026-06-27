namespace Ballcom.Order.Application.IntegrationEvents;

public record OrderPlacedItemDto(Guid ProductId, string ProductName, int Quantity);

public record OrderPlacedIntegrationEvent(Guid OrderId, Guid CustomerId, IReadOnlyCollection<OrderPlacedItemDto> Items);
