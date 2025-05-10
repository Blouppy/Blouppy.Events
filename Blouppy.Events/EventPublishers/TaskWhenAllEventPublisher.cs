using Blouppy.Events.Abstractions;
using Blouppy.Events.EventHandlers;

namespace Blouppy.Events.EventPublishers;

/// <summary>
/// Uses Task.WhenAll with the list of Handler tasks:
/// <code>
/// var tasks = handlers
///                .Select(handler => handler.Handle(@event, cancellationToken))
///                .ToList();
/// 
/// return Task.WhenAll(tasks);
/// </code>
/// </summary>
public sealed class TaskWhenAllEventPublisher : IEventPublisher
{
    public Task PublishAsync(
        IEnumerable<EventHandlerExecutor> handlerExecutors, 
        IEvent @event, 
        CancellationToken cancellationToken)
    {
        var tasks = handlerExecutors
            .Select(handler => handler.HandlerCallback(@event, cancellationToken))
            .ToArray();

        return Task.WhenAll(tasks);
    }
}
