using Blouppy.Events.Abstractions;
using Blouppy.Events.EventHandlers;
using Blouppy.Events.EventPublishers;

namespace Blouppy.Events;

internal sealed class Publisher(
    IServiceProvider serviceProvider, 
    IEventPublisher eventPublisher) : IPublisher
{
    public Task PublishAsync<TEvent>(
        TEvent @event, 
        CancellationToken cancellationToken = default)
        where TEvent : IEvent
        => PublishAsync(@event as object, cancellationToken);

    public Task PublishAsync(
        object @event, 
        CancellationToken cancellationToken = default)
    {
        if (@event is not IEvent typedEvent)
        {
            throw new ArgumentException($"Event must implement {nameof(IEvent)}");
        }

        var wrapperType = typeof(EventHandlerWrapper<>).MakeGenericType(@event.GetType());
        var wrapper = (IEventHandlerWrapper)Activator.CreateInstance(wrapperType)!;

        return wrapper.HandleAsync(typedEvent, serviceProvider, eventPublisher.PublishAsync, cancellationToken);
    }
}
