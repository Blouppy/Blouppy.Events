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
    /// Publishes an event to all handlers using a foreach loop.
    /// </summary>
    /// <param name="handlerExecutors">The list of handler executors.</param>
    /// <param name="event">The event to publish.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the publish operation.</returns>
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
