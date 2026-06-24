using Ballcom.Order.Application.Interfaces;

namespace Ballcom.Order.Infrastructure.Messaging;

public class NoOpEventPublisher : IEventPublisher
{
    public Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
