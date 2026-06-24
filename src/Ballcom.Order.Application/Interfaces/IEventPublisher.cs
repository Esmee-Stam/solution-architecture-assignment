namespace Ballcom.Order.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default);
}
