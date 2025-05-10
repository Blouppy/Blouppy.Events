using Blouppy.Events.Abstractions;
using Blouppy.Events.EventHandlers;

namespace Blouppy.Events.EventPublishers;

/// <summary>
/// Represents a publisher for events.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes an event to all handlers.
    /// </summary>
    /// <param name="handlerExecutors">The list of handler executors.</param>
    /// <param name="event">The event to publish.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the publish operation.</returns>
    Task PublishAsync(IEnumerable<EventHandlerExecutor> handlerExecutors, IEvent @event, CancellationToken cancellationToken);
}
