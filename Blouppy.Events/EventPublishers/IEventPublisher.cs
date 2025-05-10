using Blouppy.Events.Abstractions;
using Blouppy.Events.EventHandlers;

namespace Blouppy.Events.EventPublishers;

public interface IEventPublisher
{
    Task PublishAsync(IEnumerable<EventHandlerExecutor> handlerExecutors, IEvent @event, CancellationToken cancellationToken);
}
