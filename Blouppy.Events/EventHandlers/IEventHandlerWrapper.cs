using Blouppy.Events.Abstractions;

namespace Blouppy.Events.EventHandlers;

internal interface IEventHandlerWrapper
{
    Task HandleAsync(
        IEvent @event, 
        IServiceProvider serviceProvider,
        Func<IEnumerable<EventHandlerExecutor>, IEvent, CancellationToken, Task> publish,
        CancellationToken cancellationToken);
}
