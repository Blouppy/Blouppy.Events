using Blouppy.Events.Abstractions;
using Blouppy.Events.EventHandlers;

namespace Blouppy.Events.EventPublishers;

/// <summary>
/// Awaits each event handler in a single foreach loop:
/// <code>
/// foreach (var handler in handlers) {
///     await handler(@event, cancellationToken);
/// }
/// </code>
/// </summary>
public sealed class ForeachAwaitEventPublisher : IEventPublisher
{
    /// <summary>
    public async Task PublishAsync(
        IEnumerable<EventHandlerExecutor> handlerExecutors, 
        IEvent @event, 
        CancellationToken cancellationToken)
    {
        foreach (var handler in handlerExecutors)
        {
            await handler.HandlerCallback(@event, cancellationToken).ConfigureAwait(false);
        }
    }
}
