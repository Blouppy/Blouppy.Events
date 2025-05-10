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
            .Select(static x => new EventHandlerExecutor(
                HandlerInstance: x,
                HandlerCallback: (@event, token) => x.HandleAsync((TEvent)@event, token)));

        return publish(handlers, @event, cancellationToken);
    }
}
