using CliniCore.Shared.Events;

namespace CliniCore.Shared.Messaging;

public interface IMessageBroker
{
    Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default);
}