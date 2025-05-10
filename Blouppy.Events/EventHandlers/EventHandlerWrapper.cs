using Blouppy.Events.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Blouppy.Events.EventHandlers;

internal sealed class EventHandlerWrapper<TEvent> : IEventHandlerWrapper
    where TEvent : IEvent
{
    public Task HandleAsync(
        IEvent @event, 
        IServiceProvider serviceProvider,
        Func<IEnumerable<EventHandlerExecutor>, IEvent, CancellationToken, Task> publish,
        CancellationToken cancellationToken)
    {
        var handlers = serviceProvider
            .GetServices<IEventHandler<TEvent>>()
            .Select(static x => new EventHandlerExecutor(x, (theEvent, theToken) => x.HandleAsync((TEvent)theEvent, theToken)));

        return publish(handlers, @event, cancellationToken);
    }
}
