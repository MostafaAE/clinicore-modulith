using CliniCore.Shared.Events;

namespace CliniCore.Shared.Messaging;

internal interface IAsyncEventDispatcher
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, IEvent;
}